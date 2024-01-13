
interface StepCountProps
{
    currForm: number
    maxForm: number
}

const StepCount = ({currForm, maxForm}:StepCountProps) => {
  return (
    <h1 className="text-lg pb-6  font-semibold dark:text-white">Step {currForm + 1} / {maxForm +1}</h1>

  )
}

export default StepCount
