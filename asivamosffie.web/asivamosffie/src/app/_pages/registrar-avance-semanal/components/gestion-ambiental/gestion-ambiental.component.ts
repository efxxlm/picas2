import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-gestion-ambiental',
  templateUrl: './gestion-ambiental.component.html',
  styleUrls: ['./gestion-ambiental.component.scss']
})
export class GestionAmbientalComponent implements OnInit {

    @Input() esVerDetalle = false;
    formGestionAmbiental: FormGroup;
    booleanosActividadRelacionada: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];
    tipoActividades: Dominio[] = [];

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService )
    {
        this.commonSvc.listaTipoActividades()
            .subscribe(
                actividades => {
                    this.tipoActividades = actividades;
                    this.crearFormulario();
                }
            );
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        this.formGestionAmbiental = this.fb.group({
            actividadRelacionada: [ null ],
            tipoActividad: [ null ],
            manejoMaterialInsumo: this.fb.group({
                proveedores: this.fb.array( [
                    this.fb.group({
                        proveedor: [ '' ],
                        requirePermisosAmbientales: [ null ],
                        campoUrl: [ '' ]
                    })
                ] ),
                seEncontraronProtegidosCorrectamente: [ null ],
                requireObservacionMateriales: [ null ],
                observacionManejoMaterial: [ null ],
                urlSoporte: [ null ]
            }),
            manejoResiduosConstruccion: this.fb.group({
                esRcdCuantificado: [ null ],
                requiereObservacionSobreManejo: [ null ],
                observacionManejoResiduos: [ null ],
                gestorResiduos: this.fb.array( [
                    this.fb.group(
                        {
                            gestorResiduosConstruccion: [ '' ],
                            presentaPermisoAmbientalValido: [ null ],
                            urlSoporte: [ '' ]
                        }
                    )
                ] ),
                seReutilizaronLosResiduos: [ null ],
                cantidad: [ '' ]
            }),
            manejoResiduosPeligrosos: this.fb.group({
                seClasificaronlugarSeguro: [ null ],
                requiereObservacion: [ null ],
                obsManejoResiduosPeligrosos: [ null ],
                urlSoporte: [ '' ]
            }),
            otra: this.fb.group({
                fechaActividad: [ null ],
                observacionActividad: [ null ],
                urlSoporte: [ '' ]
            })
        });
    }

    valuePending( value: any ) {
        console.log( value );
    }

    guardar() {
        console.log( this.formGestionAmbiental.value );
    }

}
