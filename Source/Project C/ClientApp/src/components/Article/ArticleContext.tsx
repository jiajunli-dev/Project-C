import { createContext } from "react";
import { Article as ArticleType } from "./ArticleState";
import { Articles } from "./Articles";

export const ArticlesContext = createContext<ArticleType[]>(Articles);
