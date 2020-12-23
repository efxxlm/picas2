import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-gestion-sst',
  templateUrl: './gestion-sst.component.html',
  styleUrls: ['./gestion-sst.component.scss']
})
export class GestionSSTComponent implements OnInit {

    formSst: FormGroup;
    editorStyle = {
        height: '45px'
    };
    config = {
      toolbar: []
    };
    booleanosEnsayosLaboratorio: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];
    causasDeAccidentes: any[] = [
        { codigo: 1, viewValue: 'Incumplimiento del uso de los EPP' }
    ];
    resultadosRevision: any[] = [
        { codigo: 1, viewValue: 'Cumple' }
    ];

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog )
    {
        this.crearFormulario();
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        this.formSst = this.fb.group({
            cantidadAccidentes: [ '' ],
            principalesCausasAccidente: [ null ],
            seRealizaronCapacitaciones: [ null ],
            temaCapacitacion: [ null ],
            seRealizaronRevisiones: [ null ],
            resultadoRevision: [ null ],
            seRealizoRevisionSeñalizacion: [ null ],
            urlSoporte: [ '' ]
        });
    }

    textoLimpio(texto: string) {
        if ( texto ){
            const textolimpio = texto.replace(/<[^>]*>/g, '');
            return textolimpio.length;
        }
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
          e.editor.deleteText(n, e.editor.getLength());
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        console.log( this.formSst.value );
    }

}
