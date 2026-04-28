export interface GetPost {
  id: number;
  authorId: number;
  textContent?: string | null;
  createdAt: string;
}

export interface PostDto {
  id: number;
  authorId: number;
  textContent?: string | null;
  isDeleted: boolean;
  createdAt: string;
  likesIds?: number[] | null;
}

export interface UpdatePost {
  id: number;
  textContent?: string | null;
}