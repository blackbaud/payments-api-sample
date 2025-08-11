package com.blackbaud.paymentsapi.auth;

import java.util.concurrent.ConcurrentHashMap;

public class TokenStore {
    private static final ConcurrentHashMap<String, AuthTokens> store = new ConcurrentHashMap<>();

    public static void saveTokens(String state, AuthTokens tokens) {
        store.put(state, tokens);
    }

    public static AuthTokens getTokens(String state) {
        return store.get(state);
    }

    public static void clearTokens(String state) {
        store.remove(state);
    }
}

class AuthTokens {
    public String accessToken;
    public String refreshToken;
    public long expiresIn;
    public long refreshExpiresIn;
}
