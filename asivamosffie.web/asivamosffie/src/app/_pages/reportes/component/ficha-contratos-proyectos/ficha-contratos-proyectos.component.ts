import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
import { CommonService, Dominio, Localizacion } from 'src/app/core/_services/common/common.service';
import { ProjectService, InstitucionEducativa, ProyectoGrilla } from 'src/app/core/_services/project/project.service';
import { forkJoin } from 'rxjs';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-ficha-contratos-proyectos',
  templateUrl: './ficha-contratos-proyectos.component.html',
  styleUrls: ['./ficha-contratos-proyectos.component.scss']
})
export class FichaContratosProyectosComponent implements OnInit {

  tipoFicha: FormControl;
  
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
    private commonService: CommonService,
    private projectService: ProjectService,
    private dialog: MatDialog
    ) { }

  ngOnInit(): void {
    this.declararTipoFicha();
  }

  private declararTipoFicha() {
    this.tipoFicha = new FormControl(null, Validators.required);
  }
  
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
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
