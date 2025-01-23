import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EvalByGroup } from '../../interfaces/new-evaluation';
import { EvaluationService } from '../../services/evaluation.service';
import { Groups } from '../../interfaces/groups';
import { EvaluationCycle } from '../../interfaces/evaluation-cycles';
import { GroupsService } from '../../services/groups.service';
import { EvaluationCyclesService } from '../../services/evaluation-cycles.service';
import { CommonModule } from '@angular/common';
import { jsPDF } from 'jspdf';
import html2canvas from 'html2canvas';




@Component({
  selector: 'app-evaluations-report',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './evaluations-report.component.html',
  styleUrl: './evaluations-report.component.css'
})
export class EvaluationsReportComponent {

  evaluations: EvalByGroup[] | null = null;
  groups: Groups[] | null = null;
  cycles: EvaluationCycle[] | null = null;
  errorMessage: string = ''

  evaluationService = inject(EvaluationService)
  groupService = inject(GroupsService)
  cycleService = inject(EvaluationCyclesService)

  evalByGroup  = new FormGroup({
    groupId: new FormControl('', Validators.required),
    cycleId: new FormControl('', Validators.required)
  })

  ngOnInit(){
    // Call to get Evaluation Cycles for dropdown
    this.cycleService.getAllEvaluationCycles().subscribe({
      next: (cycle) =>{
        this.cycles = cycle;
        this.errorMessage = '';
      },
      error:(error)=>{
        this.errorMessage = 'Error while retrieving Evaluation Cycles. ';
        return;
      }
    })

    // Call to get Groups for dropdown
    this.groupService.getAllGroups().subscribe({
      next: (group) =>{
        this.groups = group;
        this.errorMessage = '';
      },
      error:(error)=>{
        this.errorMessage = 'Error while retrieving Groups. ';
        return;
      }
    })
  }

  /**
   * Called on Submit to retrieve the Evaluations
   * @param cycleId the cycle whose evaluations are retrieved
   * @param groubId the group whose evaluations are retrieved
   */
  getEvaluations(value: any){
    this.evaluationService.getEvaluationsByGroup(Number(value.groupId), Number(value.cycleId)).subscribe({
      next: (newEvalByGroup) => {
        this.evaluations = newEvalByGroup;
        console.log(this.evaluations)
      },
      error: (error: any) => {
        window.alert('An error ocurred while trying to retrive the Evaluations')
        return;
      }
    })    
  }

  // Get the dable to export it as PDF
  @ViewChild('capture') captureElement: ElementRef<HTMLDivElement> | undefined;

  exportPDF(): void {
    if (this.captureElement && this.captureElement.nativeElement) {
      html2canvas(this.captureElement.nativeElement).then(canvas => {
        const imgData = canvas.toDataURL('image/png');
        const pdf = new jsPDF({
          orientation: 'landscape',
        });
        const imgProps = pdf.getImageProperties(imgData);
        const pdfWidth = pdf.internal.pageSize.getWidth();
        const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;
        pdf.addImage(imgData, 'PNG', 0, 0, pdfWidth, pdfHeight);
        pdf.save("EvaluationsReport.pdf");
      });
    } else {
      console.error("Element #capture not found!");
    }
  }
  
}
