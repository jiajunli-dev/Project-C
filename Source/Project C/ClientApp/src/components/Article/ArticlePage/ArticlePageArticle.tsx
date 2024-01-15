import { ArticleProps } from "../ArticleState";
import { DotFilledIcon } from "../DotFilledIcon";

const ArticlePageArticle = (props: ArticleProps) => {
  return (
    <section className="w-full py-12">
      <div className="container flex flex-col gap-5 px-36">
        <h1 className="tracking-tighter text-5xl  dark:text-white">
          {props.article.title}
        </h1>
        <div className="flex gap-2  [&>span]:text-purple-400 ">
          {props.article.tags.map((tag, index, array) => (
            <span className="flex items-center gap-2" key={tag}>
              {tag}
              {index !== array.length - 1 && <DotFilledIcon />}
            </span>
          ))}
        </div>
        <div>
          <p className="text-gray-700 dark:text-gray-400">
            {props.article.content}
          </p>
        </div>
        <div className="border border-gray-200 mt-5"></div>
        <div className="[&>p]:text-gray-700 [&>p]:dark:text-gray-400 ">
          {props.article.solution.split("\n").map((line) => (
            <p>
              {line}
              <br />
            </p>
          ))}
        </div>
      </div>
    </section>
  );
};
export default ArticlePageArticle;
