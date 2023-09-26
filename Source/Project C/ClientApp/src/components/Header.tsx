import logo from '../assets/viscon_logo-removebg-preview.png'
import { useNavigate } from 'react-router-dom'

const Header = () => {

  const navigate = useNavigate()

  return (
    <div className='min-h-[60px] absolute top-0 right-0 left-0 flex items-center justify-between bg-white p-1 border-b-8 border-[#009fe3]'>
        <img src={logo} alt="logo" className="h-[50px] max-w-[150px] cursor-pointer hover:scale-95" onClick={() => navigate("/")} />
        <div className='flex justify-between w-[50%]'>
          <ul className='flex text-black gap-8 items-center text-lg'>
            <li className='cursor-pointer'>Create ticket</li> | 
            <li className='cursor-pointer'>FAQ</li> | 
            <li className='cursor-pointer'>About us</li> | 
            <li className='cursor-pointer'>Contact us</li>
          </ul>
          <svg xmlns="http://www.w3.org/2000/svg" fill="#009fe3" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-14 h-14 cursor-pointer"
            onClick={() => navigate("/authentication")}
          >
            <path strokeLinecap="round" strokeLinejoin="round" d="M17.982 18.725A7.488 7.488 0 0012 15.75a7.488 7.488 0 00-5.982 2.975m11.963 0a9 9 0 10-11.963 0m11.963 0A8.966 8.966 0 0112 21a8.966 8.966 0 01-5.982-2.275M15 9.75a3 3 0 11-6 0 3 3 0 016 0z" />
          </svg>
        </div>


  </div>
  )
}

export default Header
