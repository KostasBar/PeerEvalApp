export interface PastUserEvaluationCycles {
    cycleId: number,
    department: string,
    cycleStartDate: Date,
    cycleEndDate: Date,
    avgEvaluationResult: number

}

export interface EvaluationCycle {
    cycleId: number,
    cycleStartDate: Date,
    cycleEndDate: Date,
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