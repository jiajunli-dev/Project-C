import Article from "@/components/Article/Article";
import { Link } from "react-router-dom";
import { Articles } from "../components/Article/Articles";
const ArticlesPage = () => {
  return (
    <main className="dark:bg-background">
      <section className="w-full py-12 md:py-18 lg:py-22 ">
        <div className="container px-4 md:px-36">
          <h1 className="text-3xl tracking-tighter sm:text-4xl md:text-5xl lg:text-6xl/none dark:text-white">
            Popular Articles
          </h1>

          {Articles.map((article) => (
            <div className="w-[50rem]" key={article.id}>
              <Link to={`/article/${article.id}`}>
                <Article article={article} />
              </Link>
            </div>
          ))}
        </div>
      </section>
    </main>
  );
};
export default ArticlesPage;
