import { useContext, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Article as ArticleType } from "../ArticleState";
import { ArticlesContext } from "../ArticleContext";
import ArticlePageArticle from "./ArticlePageArticle";

const ArticlePage = () => {
  const articles = useContext(ArticlesContext);

  const { id } = useParams<{ id: string }>();
  const [article, setArticle] = useState<ArticleType | null>(null);

  useEffect(() => {
    const article = articles.find((article) => article.id === id);
    setArticle(article || null);
  }, [id]);

  if (!article) {
    return <div>Article not found</div>;
  }

  return <ArticlePageArticle article={article} />;
};

export default ArticlePage;
