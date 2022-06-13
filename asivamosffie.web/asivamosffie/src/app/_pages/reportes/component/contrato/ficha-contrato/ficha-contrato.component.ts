import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { FichaContratoService } from 'src/app/core/_services/fichaContrato/ficha-contrato.service';

 
@Component({
  selector: 'app-ficha-contrato',
  templateUrl: './ficha-contrato.component.html',
  styleUrls: ['./ficha-contrato.component.scss']
})
export class FichaContratoComponent implements OnInit {

  verResultados = false;
  mostrarFicha = false;
  contratosArray = [];
  addressForm: FormGroup = this.fb.group({
    numeroContrato: [null],
    nombreContratista: [null],
    departamento: [null],
    municipio: [null],
    institucionEducativa: [null],
    dede: [null],
    tipoContrato: [null],
    vigenciaContratación: [null]
  });

  listaTipoFicha = [
    {
      name: 'Ficha de contrato',
      value: 'Ficha de contrato'
    },
    {
      name: 'Ficha de proyecto',
      value: 'Ficha de proyecto'
    }
  ]

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private fichaContratoService: FichaContratoService
  ) { }

  ngOnInit(): void {
  }

  reiniciarFiltro() {
    this.addressForm.setValue({
      numeroContrato: null,
      nombreContratista: null,
      departamento: null,
      municipio: null,
      institucionEducativa: null,
      dede: null,
      tipoContrato: null,
      vigenciaContratación: null
    });
  }
  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  getContratos(trigger: string) {
    if(trigger != null && trigger != undefined){
      if(trigger.length >= 3){
        /*this.limpiarListas(); */
        this.fichaContratoService.getContratosByNumeroContrato(trigger)
        .subscribe(response => {
          this.contratosArray = response;
          if ( response.length === 0 ) {
            this.openDialog( '', '<b>No se encontró un numero de contrato relacionado en la búsqueda.</b>' );
          }
        });
      }
    }
  }

  buscar() {
    this.verResultados = true;
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.buscar();
  }

  verFicha(ficha: boolean) {
    console.log(ficha);
    this.mostrarFicha = ficha;
  }

}
