export interface NewEvaluation {
    evaluatorUserId: number,
    evaluateeUserId: number,
    evaluationCycleId: number,
    timeStamp: string,
    answers: [{
        questionId: number,
        answerValue: string
    }]
}

export interface EvalByGroup {
    cycleId: number,
    cycleTitle: string,
    evaluateeFirstName: string,
    evaluateeLastName: string,
    average: number
}