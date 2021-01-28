import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { TipoDDP } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-con-validacion-presupuestal',
  templateUrl: './con-validacion-presupuestal.component.html',
  styleUrls: ['./con-validacion-presupuestal.component.scss']
})
export class ConValidacionPresupuestalComponent implements OnInit {

  constructor(public dialog: MatDialog,private disponibilidadServices: DisponibilidadPresupuestalService,
    private route: ActivatedRoute,
    private router: Router,private sanitized: DomSanitizer,
    ) { }
    detailavailabilityBudget:any=null;
    esModificacion=false;
    pTipoDDP=TipoDDP;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.disponibilidadServices.GetDetailAvailabilityBudgetProyect(id).subscribe(listas => {
        console.log(listas);
        this.detailavailabilityBudget=listas[0];
      });
    }
  }
  download()
  {
    console.log(this.detailavailabilityBudget);
    this.disponibilidadServices.GenerateDDP(this.detailavailabilityBudget.id).subscribe((listas:any) => {
      console.log(listas);
      const documento = `${ this.detailavailabilityBudget.numeroDDP  }.pdf`;
        const text = documento,
          blob = new Blob([listas], { type: 'application/pdf' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef= this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      if(result === true)
      {
       
      }
    });
  }

}
