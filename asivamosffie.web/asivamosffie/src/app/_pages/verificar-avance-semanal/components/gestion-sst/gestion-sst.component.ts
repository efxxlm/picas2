import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-gestion-sst',
  templateUrl: './gestion-sst.component.html',
  styleUrls: ['./gestion-sst.component.scss']
})
export class GestionSstComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    seguimientoSemanalGestionObraSeguridadSaludId = 0;
    gestionObraSst: any;
    editorStyle = {
        height: '45px'
    };
    config = {
      toolbar: []
    };
    causasDeAccidentes: Dominio[] = [];

    constructor(
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private routes: Router )
    { }

    ngOnInit(): void {
        this.getGestionSst();
    }

    getGestionSst() {
        this.commonSvc.listaCausaAccidente()
        .subscribe( response => {
            this.causasDeAccidentes = response;
            if ( this.seguimientoSemanal !== undefined ) {
                this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
                this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
                this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;

                if (    this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0
                    && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSeguridadSalud.length > 0 )
                {
                    this.gestionObraSst =
                        this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSeguridadSalud[0];
                }
            }
        } );
    }

    getCausasDetalle( causas: any[] ) {
        if ( this.causasDeAccidentes.length > 0 && this.seguimientoSemanal !== undefined ) {
            const causaSeleccion = [];
            causas.forEach( causa => {
                this.causasDeAccidentes.filter( value => {
                    if ( causa.causaAccidenteCodigo === value.codigo ) {
                        causaSeleccion.push( value );
                    }
                } );
            } );
            return causaSeleccion;
        }
    }

}
