import { Outlet } from 'react-router-dom'

const Parent = () => {
  return (
      <div className=''>
        <Outlet />
      </div>
  )
}

export default Parent