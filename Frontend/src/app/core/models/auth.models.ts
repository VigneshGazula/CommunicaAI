export interface AuthResponse {
  userId: string;
  fullName: string;
  email: string;
  token: string;
  expiresAtUtc: string;
}

export interface UserProfile {
  id: string;
  email: string;
  fullName: string;
}
