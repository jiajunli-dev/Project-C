import { useState, useEffect, Children } from "react"
import { useNavigate } from "react-router-dom"

interface Props{
  children: JSX.Element[],

}

export default function Carousel({
  children: slides,
}:Props) 

{
  const [curr, setCurr] = useState(0)

  const prev = () =>
    setCurr((curr) => (curr === 0 ? slides.length - 1 : curr - 1))
  const next = () =>
    setCurr((curr) => (curr === slides.length - 1 ? 0 : curr + 1))

  return (
    <div className="relative w-full h-full">

    <div className="overflow-hidden h-full relative rounded-xl bg-gray-200 border flex items-center justify-center ">
      <div
        className="flex transition-transform ease-out duration-500"
        style={{ transform: `translateX(-${curr * 100.5}%)` }}
      >
        {slides}
      </div>

      {slides.length > 1 && (
        <div className="absolute inset-0 flex items-center justify-between p-4">
          <button
            onClick={prev}
            className="p-1 rounded-full shadow bg-white/80 text-gray-800 hover:bg-white"
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">
                <path strokeLinecap="round" strokeLinejoin="round" d="M6.75 15.75 3 12m0 0 3.75-3.75M3 12h18" />
            </svg>

          </button>
          <button
            onClick={next}
            className="p-1 rounded-full shadow bg-white/80 text-gray-800 hover:bg-white"
          >
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">
            <path strokeLinecap="round" strokeLinejoin="round" d="M17.25 8.25 21 12m0 0-3.75 3.75M21 12H3" />
        </svg>

          </button>
      </div>
      )}
      

    </div>
    <div className="absolute bottom-4 right-0 left-0 bg-transparent">
    <div className="flex items-center justify-center gap-2">
      {slides.map((_, i) => (
        <img key={i}
          onClick={() => setCurr(i)}
          className={`
          transition-all w-12 h-12 rounded-sm object-cover cursor-pointer
          ${curr !== i &&"opacity-70"}
        `}
          src={slides[i].props.src}
        />
      ))}
    </div>
    </div>
  </div>
  )
}      
