export interface PastUserEvaluationCycles {
    cycleId: number,
    department: string,
    cycleStartDate: Date,
    cycleEndDate: Date,
    avgEvaluationResult: number

}

export interface EvaluationCycle {
    id: number,
    title: string,
    startDate: Date,
    endDate: Date,
    status: number
}

export interface UpdateCycle{
    id: number,
    status: number,
    endWeek: number
}

export interface InitializeCycle{
    title:string,
    weeks: number
}