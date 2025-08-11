package com.blackbaud.paymentsapi.auth;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;
import org.springframework.http.ResponseEntity;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import java.util.*;

@Service
public class AuthService {
    @Value("${auth.clientId}")
    private String clientId;
    @Value("${auth.redirectUri}")
    private String redirectUri;
    @Value("${auth.baseUri}")
    private String baseUri;

    private final RestTemplate restTemplate = new RestTemplate();

    public Map<String, String> buildAuthorizationUri() {
        String state = UUID.randomUUID().toString();
        String codeVerifier = PkceUtil.generateCodeVerifier();
        String codeChallenge = PkceUtil.generateCodeChallenge(codeVerifier);
        String uri = baseUri + "authorization?response_type=code&code_challenge_method=S256&client_id=" + clientId + "&redirect_uri=" + redirectUri + "&code_challenge=" + codeChallenge + "&state=" + state;
        // Save verifier for later
        TokenStore.saveTokens(state, new AuthTokens() {{ accessToken = null; refreshToken = null; }});
        TokenStore.saveTokens(state + "_verifier", new AuthTokens() {{ accessToken = codeVerifier; }});
        Map<String, String> result = new HashMap<>();
        result.put("uri", uri);
        result.put("state", state);
        return result;
    }

    public AuthTokens exchangeCodeForTokens(String code, String state) {
        AuthTokens verifierToken = TokenStore.getTokens(state + "_verifier");
        if (verifierToken == null || verifierToken.accessToken == null) throw new RuntimeException("Invalid state or verifier");
        String codeVerifier = verifierToken.accessToken;
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_FORM_URLENCODED);
        Map<String, String> body = new HashMap<>();
        body.put("code", code);
        body.put("grant_type", "authorization_code");
        body.put("redirect_uri", redirectUri);
        body.put("code_verifier", codeVerifier);
        StringBuilder sb = new StringBuilder();
        for (Map.Entry<String, String> entry : body.entrySet()) {
            sb.append(entry.getKey()).append("=").append(entry.getValue()).append("&");
        }
        HttpEntity<String> entity = new HttpEntity<>(sb.toString(), headers);
        ResponseEntity<Map> response = restTemplate.postForEntity(baseUri + "token", entity, Map.class);
        Map resp = response.getBody();
        AuthTokens tokens = new AuthTokens();
        tokens.accessToken = (String) resp.get("access_token");
        tokens.refreshToken = (String) resp.get("refresh_token");
        tokens.expiresIn = ((Number) resp.get("expires_in")).longValue();
        tokens.refreshExpiresIn = ((Number) resp.get("refresh_token_expires_in")).longValue();
        TokenStore.saveTokens(state, tokens);
        TokenStore.clearTokens(state + "_verifier");
        return tokens;
    }

    public AuthTokens refreshAccessToken(String state) {
        AuthTokens tokens = TokenStore.getTokens(state);
        if (tokens == null || tokens.refreshToken == null) throw new RuntimeException("No refresh token");
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_FORM_URLENCODED);
        Map<String, String> body = new HashMap<>();
        body.put("grant_type", "refresh_token");
        body.put("refresh_token", tokens.refreshToken);
        body.put("preserve_refresh_token", "true");
        StringBuilder sb = new StringBuilder();
        for (Map.Entry<String, String> entry : body.entrySet()) {
            sb.append(entry.getKey()).append("=").append(entry.getValue()).append("&");
        }
        HttpEntity<String> entity = new HttpEntity<>(sb.toString(), headers);
        ResponseEntity<Map> response = restTemplate.postForEntity(baseUri + "token", entity, Map.class);
        Map resp = response.getBody();
        tokens.accessToken = (String) resp.get("access_token");
        tokens.refreshToken = (String) resp.get("refresh_token");
        tokens.expiresIn = ((Number) resp.get("expires_in")).longValue();
        tokens.refreshExpiresIn = ((Number) resp.get("refresh_token_expires_in")).longValue();
        TokenStore.saveTokens(state, tokens);
        return tokens;
    }
}
