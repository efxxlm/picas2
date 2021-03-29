import { Dominio } from './../../../../core/_services/common/common.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { OrdenPagoService } from './../../../../core/_services/ordenPago/orden-pago.service';
import { Component, Input, OnInit } from '@angular/core';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-origen',
  templateUrl: './form-origen.component.html',
  styleUrls: ['./form-origen.component.scss']
})
export class FormOrigenComponent implements OnInit {

    @Input() solicitudPago: any;
    formOrigen: FormGroup;
    listaNombreCuenta: Dominio[] = [ { codigo: '1', nombre: 'Alcaldía de Susacón' } ];

    constructor(
        private ordenGiroSvc: OrdenPagoService,
        private dialog: MatDialog,
        private fb: FormBuilder )
    {
        this.formOrigen = this.crearFormulario();
    }

    ngOnInit(): void {

    }

    crearFormulario() {
        return this.fb.group(
            {
                nombreCuenta: [ null, Validators.required ]
            }
        )
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        console.log( this.formOrigen );
    }

}
