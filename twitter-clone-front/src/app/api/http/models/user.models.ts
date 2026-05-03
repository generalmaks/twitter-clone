export interface GetUser {
  id: number;
  username?: string | null;
  displayUsername?: string | null;
  bio?: string | null;
  createdAt: string;
}

export interface LoginUser {
  email?: string | null;
  password?: string | null;
}

export interface LoginResponse {
  userId: number;
  token: string;
}

export interface RegisterUser {
  username?: string | null;
  displayUsername?: string | null;
  email?: string | null;
  unhashedPassword?: string | null;
  bio?: string | null;
}

export interface UpdateUser {
  id: number;
  username?: string | null;
  displayUsername?: string | null;
  bio?: string | null;
  password?: string | null;
}
