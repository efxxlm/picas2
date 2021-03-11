import { ActivatedRoute, UrlSegment } from '@angular/router';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';

@Component({
  selector: 'app-ver-detalle-avance-semanal',
  templateUrl: './ver-detalle-avance-semanal.component.html',
  styleUrls: ['./ver-detalle-avance-semanal.component.scss']
})
export class VerDetalleAvanceSemanalComponent implements OnInit {

    seguimientoSemanal: any;
    tipoObservaciones: any;
    semaforoGestionObra = 'sin-diligenciar';
    esVerDetalleMuestras: any;

    constructor(
        private location: Location,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private activatedRoute: ActivatedRoute )
    {
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
            if ( urlSegment.path === 'verDetalleAvanceSemanal' ) {
                this.esVerDetalleMuestras = true;
            }
            if ( urlSegment.path === 'verDetalleAvanceSemanalMuestras' ) {
                this.esVerDetalleMuestras = false;
            }
        } );
        this.avanceSemanalSvc.getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( 0,  this.activatedRoute.snapshot.params.idAvance )
            .subscribe(
                seguimiento => {
                    this.verificarAvanceSemanalSvc.tipoObservaciones()
                        .subscribe( tipoObservaciones => {
                            this.tipoObservaciones = tipoObservaciones;
                            this.seguimientoSemanal = seguimiento;
                            console.log( this.seguimientoSemanal );
                        } )
                }
            );
    }

    ngOnInit(): void {
    }

    getRutaAnterior() {
        this.location.back();
    }

    valuePendingSemaforo( value: string, tipoSemaforo: string ) {
        if ( tipoSemaforo === 'gestionObra' ) {
          this.semaforoGestionObra = value;
          
        }
    }

    valuePending( value: number ) {
        if ( value % 5 === 0 ) {
            return '';
        } else {
            return 'en-alerta';
        }
    }


}
