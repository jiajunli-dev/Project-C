import { Outlet } from 'react-router-dom'

const Parent = () => {
  return (
      <div className='mt-[60px]'>
        <Outlet />
      </div>
  )
}

export default Parent