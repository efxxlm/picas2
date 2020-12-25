import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-comite-obra',
  templateUrl: './comite-obra.component.html',
  styleUrls: ['./comite-obra.component.scss']
})
export class ComiteObraComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    numeroComiteObra: string;
    seguimientoSemanalId: number;
    seguimientoSemanalRegistrarComiteObraId: number;
    gestionComiteObra: any;

    constructor() { }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalRegistrarComiteObraId =  this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra[0].seguimientoSemanalRegistrarComiteObraId : 0;
            if ( this.seguimientoSemanal.comiteObraGenerado !== undefined ) {
                this.numeroComiteObra = this.seguimientoSemanal.comiteObraGenerado;
            }

            if ( this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra.length > 0 ) {
                this.gestionComiteObra = this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra[0];
                this.numeroComiteObra = this.gestionComiteObra.numeroComite;
            }
        }
    }

}
