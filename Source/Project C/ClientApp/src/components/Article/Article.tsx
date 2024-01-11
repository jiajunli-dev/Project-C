import { ArticleProps } from "./ArticleState";
import { DotFilledIcon } from "./DotFilledIcon";
import { FileQuestionIcon } from "./FileQuestionIcon";

const Article = (props: ArticleProps): JSX.Element => {
  return (
    <div className="flex flex-col gap-6 mt-8  w-[50rem] ">
      <div className="flex items-start gap-4   cursor-pointer [&>div>h2]:hover:text-black">
        <FileQuestionIcon className="h-6 w-6 flex-shrink-0" />
        <div>
          <h2 className="text-lg font-bold text-purple-800 dark:text-white">
            {props.article.title}
          </h2>
          <p className="text-gray-500 dark:text-gray-400">
            {props.article.content}
          </p>
          <div className="flex gap-2 [&>span]:text-purple-400 ">
            {props.article.tags.map((tag, index, array) => (
              <span
                className="flex items-center gap-2"
                key={tag}
              >
                {tag}
                {index !== array.length - 1 && <DotFilledIcon />}
              </span>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Article;


