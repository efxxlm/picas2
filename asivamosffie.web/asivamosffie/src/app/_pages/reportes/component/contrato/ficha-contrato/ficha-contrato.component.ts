import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-ficha-contrato',
  templateUrl: './ficha-contrato.component.html',
  styleUrls: ['./ficha-contrato.component.scss']
})
export class FichaContratoComponent implements OnInit {

  verResultados = false;
  mostrarFicha = false;

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
    private dialog: MatDialog
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
