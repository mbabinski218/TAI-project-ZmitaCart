export interface UserRegister {
  email: string,
  firstName: string,
  lastName: string,
  password: string,
  confirmedPassword: string,
}

export interface UserLogin {
  email: string,
  password: string,
}

export interface UserAuthorization {
  accessToken: string,
  refreshToken: string,
}

export interface TokenData {
  email: string,
  firstName: string,
  id: string,
  lastName: string,
  role: string,
  iss: string,
  exp?: number,
  expires_at?: string,
  refreshToken?: string,
}
