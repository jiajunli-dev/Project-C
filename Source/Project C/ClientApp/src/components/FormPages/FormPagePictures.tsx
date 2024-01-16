import { useEffect, useState } from 'react'
import ImageDisplay from './ImageDisplay'
import StepCount from './StepCount'
import { finished } from 'stream'

interface Props {
    setCurrentShow: React.Dispatch<React.SetStateAction<number>>
    setFinishedImages: React.Dispatch<React.SetStateAction<string[] | []>>
    maxForm: number
    finishedImages: string[]
    currentShow: number
}

const UploadImages = ({ setCurrentShow, setFinishedImages, currentShow, finishedImages, maxForm }: Props) => {
    const [addImages, setAddImages] = useState<boolean>(false)


    const handleNewImage = async (e: React.ChangeEvent<HTMLInputElement>) => {
        if (finishedImages.length > 6) return
        e.preventDefault()
        const file = e.target.files![0]
        const base64 = await convertToBase64(file)
        setFinishedImages([...finishedImages, base64 as string])
        e.target.value = '';
    }
    

    return (

        <div className="w-3/5 border-2 p-4 dark:bg-[#121212]">
        <StepCount currForm={currentShow} maxForm={maxForm} />
  

        {addImages ? (
        <div>
            <h1 className="text-black text-lg font-bold mb-8 dark:text-white">
                Upload images to clarify the issue even further!
            </h1>
            <div className='flex gap-2 '>
                <button className='bg-blue-500 font-bold md:p-2 p-1 mt-4 rounded w-fit'>
                    <label className='w-full p-1 text-white'>
                        Upload
                        <input type="file" name='image' id='file-upload'
                            accept='.jpeg, .png, .jpg'
                            onChange={(e) => handleNewImage(e)}
                            className='hidden w-full'
                        />
                    </label>

                </button>
                <button  className='bg-red-500 text-white font-bold md:p-2 p-1 mt-4 rounded'
                    onClick={() => setFinishedImages([])}
                >
                    Delete
                </button>
            </div>


            <p className='text-gray-500'>{finishedImages.length} / 7</p>
            <ImageDisplay images={finishedImages} />

        </div>) : 
        (
        <div className='flex items-center justify-center flex-col'>
            <h1 className="text-black text-sm font-bold mb-4 dark:text-white">
            If you want to add images to clarify the issue even further, click the button below! Else, click next.
            </h1>
            <button onClick={() => setAddImages(true)}
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
                Add Images 
            </button>
       </div>
        )}
      </div>

    )
}

export default UploadImages

function convertToBase64(file: File) {
    return new Promise((resolve, reject) => {
        const fileReader = new FileReader();
        fileReader.readAsDataURL(file);
        fileReader.onload = () => {
            resolve(fileReader.result)
        }
        fileReader.onerror = (error) => {
            reject(error)
        }
    })
}