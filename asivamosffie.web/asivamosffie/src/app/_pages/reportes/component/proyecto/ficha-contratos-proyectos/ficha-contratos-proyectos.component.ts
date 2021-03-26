import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
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
    this.declararTipoFicha();
  }

  private declararTipoFicha() {
    this.tipoFicha = new FormControl(null, Validators.required);
  }
  

}
