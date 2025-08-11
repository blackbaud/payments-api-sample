package com.blackbaud.paymentsapi.auth;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/auth")
public class AuthController {
    @Autowired
    private AuthService authService;

    @GetMapping("/login")
    public LoginResponse login() {
        // Returns the authorization URI and state
        var result = authService.buildAuthorizationUri();
        return new LoginResponse(result.get("uri"), result.get("state"));
    }

    @PostMapping("/callback")
    public AuthTokens callback(@RequestParam String code, @RequestParam String state) {
        // Exchanges code for tokens
        return authService.exchangeCodeForTokens(code, state);
    }

    @PostMapping("/refresh")
    public AuthTokens refresh(@RequestParam String state) {
        // Refreshes access token
        return authService.refreshAccessToken(state);
    }
}

class LoginResponse {
    public String uri;
    public String state;
    public LoginResponse(String uri, String state) {
        this.uri = uri;
        this.state = state;
    }
}
