import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-expansion-panel',
  templateUrl: './expansion-panel.component.html',
  styleUrls: ['./expansion-panel.component.scss']
})
export class ExpansionPanelComponent implements OnInit {

  @Input() novedad: NovedadContractual
  @Input() tieneAdicion: boolean

  constructor(
    private contractualNoveltyService : ContractualNoveltyService,
    private dialog: MatDialog,
    private router: Router,
    
  ) { }

  ngOnInit(): void {
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  guardar(){
    this.contractualNoveltyService.createEditNovedadContractualTramite( this.novedad )
      .subscribe( respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        if ( respuesta.code === '200' )
          this.router.navigate(['/gestionarTramiteNovedadesContractualesAprobadas']);
      });

  }

}
