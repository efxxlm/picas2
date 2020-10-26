import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Contrato, TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-form-verificacion-requisitos',
  templateUrl: './form-verificacion-requisitos.component.html',
  styleUrls: ['./form-verificacion-requisitos.component.scss']
})
export class FormVerificacionRequisitosComponent implements OnInit {

  contrato: Contrato;
  fechaPoliza: string;

  constructor(
    private faseUnoConstruccionService: FaseUnoConstruccionService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,) {
    this.getContrato();

    if (this.router.getCurrentNavigation().extras.state)
      this.fechaPoliza = this.router.getCurrentNavigation().extras.state.fechaPoliza;
  }

  ngOnInit(): void {
  }

  openDialog (modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  };

  getContrato() {
    this.faseUnoConstruccionService.getContratoByContratoId(this.activatedRoute.snapshot.params.id)
      .subscribe(response => {
        this.contrato = response;
        console.log(this.contrato);
        
      });
  };

  Cargar( seGuardo: boolean ){
    if ( seGuardo )
      this.getContrato();      
  }
    

}
