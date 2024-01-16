
export type Article = {
  id: string;
  title: string;
  content: string;
  solution: string;
  tags: string[];
};

export type ArticleProps = { article: Article };