export interface CreatePost {
  replyToPostId?: number | null;
  textContent?: string | null;
}

export interface GetPost {
  id: number;
  authorId: number;
  textContent?: string | null;
  replyToPostId?: number | null;
  createdAt: string;
}

export interface PostDto {
  id: number;
  authorId: number;
  replyToPostId?: number | null;
  textContent?: string | null;
  isDeleted: boolean;
  createdAt: string;
  likesIds?: number[] | null;
}
