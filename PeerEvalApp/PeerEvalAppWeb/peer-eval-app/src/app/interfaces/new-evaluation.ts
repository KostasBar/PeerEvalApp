export interface NewEvaluation {
    evaluatorUserId: number,
    evaluateeUserId: number,
    evaluationCycleId: number,
    timeStamp: string,
    answers:[{
        questionId: number,
        answerValue: string
    }]
}
