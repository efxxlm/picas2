import { MatTableDataSource } from '@angular/material/table';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogAvanceAcumuladoComponent } from '../dialog-avance-acumulado/dialog-avance-acumulado.component';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-avance-fisico',
  templateUrl: './tabla-avance-fisico.component.html',
  styleUrls: ['./tabla-avance-fisico.component.scss']
})
export class TablaAvanceFisicoComponent implements OnInit {

    @Input() esVerDetalle = false;
    tablaAvanceFisico = new MatTableDataSource();
    displayedColumns: string[]  = [
        'semanaNumero',
        'periodoReporte',
        'programacionSemana',
        'capitulo',
        'programacionCapitulo',
        'avanceFisicoCapitulo',
        'avanceFisicoSemana'
    ];
    dataTable: any[] = [
        {
            semanaNumero: 1,
            periodoReporte: '04/07/2020 - 11/07/2020',
            programacionSemana: '2%',
            capitulo: 'Preliminares',
            programacionCapitulo: '2%',
            avanceFisicoCapitulo: null,
            avanceFisicoSemana: ''
        }
    ];

    constructor( private dialog: MatDialog ) { }

    ngOnInit(): void {
        this.tablaAvanceFisico = new MatTableDataSource( this.dataTable );
    }

    valuePending( value: string, registro: any ) {
        if ( Number( value ) > 100 ) {
            registro.avanceFisicoSemana = '100';
        } else if ( Number( value ) < 0 ) {
            registro.avanceFisicoSemana = '0';
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data : { modalTitle, modalText }
        });
    }

    openDialogObservaciones( ) {
        this.dialog.open( DialogAvanceAcumuladoComponent, {
            width: '80em'
        } );
    }

    guardar() {
        this.openDialog( '', '<b>La información ha sido guardada exitosamente.</b>' );
    }

}
