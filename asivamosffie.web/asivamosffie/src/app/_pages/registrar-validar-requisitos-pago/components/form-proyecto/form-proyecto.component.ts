import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { TiposDeFase } from 'src/app/_interfaces/solicitud-pago.interface';

@Component({
  selector: 'app-form-proyecto',
  templateUrl: './form-proyecto.component.html',
  styleUrls: ['./form-proyecto.component.scss']
})
export class FormProyectoComponent implements OnInit {

    @Input() proyecto: FormGroup;
    @Input() contrato: any;
    @Input() listaMenusId: any;
    @Input() registrarSolicitudPagoObs: any;
    listaFases: Dominio[] = [];
    fasesContrato = TiposDeFase;
    postConstruccion = '3';
    solicitudPagoCargarFormaPago: any;
    addressForm = this.fb.group(
        {
            fase: [ null, Validators.required ],
            fases: this.fb.array( [] )
        }
    );

    get fases() {
        return this.addressForm.get( 'fases' ) as FormArray
    };

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService )
    { }

    ngOnInit(): void {
        console.log( this.proyecto )
        this.getDataProyecto()
    }

    async getDataProyecto() {
        if ( this.contrato.solicitudPago.length > 1 ) {
            this.solicitudPagoCargarFormaPago = this.contrato.solicitudPago[0].solicitudPagoCargarFormaPago[0];
        } else {
            this.solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];
        }

        const LISTA_FASES = await this.commonSvc.listaFases().toPromise()

        LISTA_FASES.forEach( ( fase, index ) => {
            if ( fase.codigo === this.postConstruccion ) {
                LISTA_FASES.splice( index, 1 );
            }
            if ( ( this.solicitudPagoCargarFormaPago.tieneFase1 === false || this.solicitudPagoCargarFormaPago.tieneFase1 === undefined ) && this.fasesContrato.preConstruccion === fase.codigo ) {
                LISTA_FASES.splice( index, 1 );
            }
        } );

        this.listaFases = LISTA_FASES
    }

    getValueFase( listFaseCodigo: Dominio[] ) {
        const listaFase = [ ...listFaseCodigo ];

        if ( listaFase.length === 0 ) {
            this.fases.clear()
            return
        }

        if ( this.fases.length > 0 ) {
            this.fases.controls.forEach( ( faseControl, indexControl ) => {
                const FASE_INDEX = listaFase.findIndex( value => value.codigo === faseControl.get( 'codigo' ).value );

                if ( FASE_INDEX !== -1 ) {
                    listaFase.splice( FASE_INDEX, 1 )
                } else {
                    this.fases.removeAt( indexControl )
                }
            } )

            for ( const fase of listaFase ) {
                this.fases.push(
                    this.fb.group(
                        {
                            codigo: fase.codigo,
                            nombre: fase.nombre,
                            contratacionProyectoId: this.proyecto.get( 'contratacionProyectoId' ).value,
                            llaveMen: this.proyecto.get( 'llaveMen' ).value
                        }
                    )
                )
            }
        } else {
            for ( const fase of listaFase ) {
                this.fases.push(
                    this.fb.group(
                        {
                            codigo: fase.codigo,
                            nombre: fase.nombre,
                            contratacionProyectoId: this.proyecto.get( 'contratacionProyectoId' ).value,
                            llaveMen: this.proyecto.get( 'llaveMen' ).value
                        }
                    )
                )
            }
        }
    }

}
