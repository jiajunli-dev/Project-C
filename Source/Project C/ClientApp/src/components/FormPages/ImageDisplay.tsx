interface Props {
    images: string[] | []
}

const ImageDisplay = ({images}:Props) => {

  return (
    <div className='mt-2'>
        <div className='flex md:flex-row flex-col gap-4 mx-auto'>

            {images[0] ? (
                <img src={images[0]} alt="image" className='h-40 w-40 object-cover'/>
            ) : (
                <div className='white border-dotted border-black  border-2 h-40 w-40 rounded-md'></div>
            )} 
            <div className='grid grid-rows-2 grid-cols-3 gap-1'>
                {images[1] ? (
                    <img src={images[1]} alt="image" className='h-20 w-20 object-cover'/>
                ):(
                    <div className='white border-dotted border-black  border-2 h-20 w-20 rounded-md'></div>
                )}
                {images[2] ? (
                    <img src={images[2]} alt="image" className='h-20 w-20 object-cover'/>
                ):(
                    <div className='white border-dotted border-black  border-2 h-20 w-20 rounded-md'></div>
                )}
                {images[3] ? (
                    <img src={images[3]} alt="image" className='h-20 w-20 object-cover'/>
                ):(
                    <div className='white border-dotted border-black  border-2 h-20 w-20 rounded-md'></div>
                )}
                {images[4] ? (
                    <img src={images[4]} alt="image" className='h-20 w-20 object-cover'/>
                ):(
                    <div className='white border-dotted border-black  border-2 h-20 w-20 rounded-md'></div>
                )}
                {images[5] ? (
                    <img src={images[5]} alt="image" className='h-20 w-20 object-cover'/>
                ):(
                    <div className='white border-dotted border-black  border-2 h-20 w-20 rounded-md'></div>
                )}
                {images[6] ? (
                    <img src={images[6]} alt="image" className='h-20 w-20 object-cover'/>
                ):(
                    <div className='white border-dotted border-black  border-2 h-20 w-20 rounded-md'></div>
                )}
            </div>

        </div>
    </div>
  )
}

export default ImageDisplay