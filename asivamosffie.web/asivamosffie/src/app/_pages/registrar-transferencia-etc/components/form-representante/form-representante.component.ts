import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RepresentanteETCRecorrido } from 'src/app/_interfaces/proyecto-entrega-etc';

@Component({
  selector: 'app-form-representante',
  templateUrl: './form-representante.component.html',
  styleUrls: ['./form-representante.component.scss']
})
export class FormRepresentanteComponent implements OnInit {

  @Input() numRepresentantesRecorrido: any;
  @Input() proyectoEntregaEtcId: any;
  @Input() representanteEtcrecorrido: any;

  representantesForm: FormGroup;
  ELEMENT_DATA : RepresentanteETCRecorrido[] = [];
  dataSource = new MatTableDataSource<RepresentanteETCRecorrido>(this.ELEMENT_DATA);
  estaEditando = false;

  get representantes() {
    return this.representantesForm.get("representantes") as FormArray
  }
  
  constructor(private fb: FormBuilder, public dialog: MatDialog) {
    this.crearFormulario();
  }

  crearFormulario() {
    this.representantesForm = this.fb.group({
        numRepresentantesRecorrido: [ '' ],
        representantes: this.fb.array( [] )
    });
  }

  ngOnInit(): void {
    if(this.numRepresentantesRecorrido != null){
        if ( this.representanteEtcrecorrido !== undefined && this.representanteEtcrecorrido.length > 0 ) {
          this.representantes.clear();
          for ( const representante of this.representanteEtcrecorrido ) {
            console.log(representante);
              this.representantes.push(
                  this.fb.group(
                      {
                          representanteEtcid: representante.representanteEtcid,
                          proyectoEntregaEtcId: representante.proyectoEntregaEtcId,
                          nombre: representante.nombre,
                          cargo:  representante.cargo,
                          dependencia: representante.dependencia,
                          semaforo: representante.registroCompleto == true ? "completo" : 
                                    representante.registroCompleto == false && (representante.nombre == "" || representante.nombre == null || representante.cargo == "" || representante.cargo == null || representante.dependencia == "" || representante.dependencia == null) ? "en-proceso" : "sin-diligenciar"
                      }
                  )
              );
          }
        }else{
            for ( let i = 0; i < this.numRepresentantesRecorrido; i++ ) {
              this.representantes.push(
                  this.fb.group({
                    representanteEtcid: [null, Validators.required],
                    proyectoEntregaEtcId: [this.proyectoEntregaEtcId, Validators.required],  
                    nombre: [null, Validators.required],
                    cargo: [null, Validators.required],
                    dependencia: [null, Validators.required],
                    semaforo: "sin-diligenciar"
                  })
              );
          }
        }
    }
  }

  ngOnChanges(changes) {
    if(changes.numRepresentantesRecorrido.currentValue != null){
      this.representantesForm.get("numRepresentantesRecorrido").setValue(this.numRepresentantesRecorrido, { emitEvent: true });
      this.representantesForm.markAsDirty();
      this.getCantidadRepresentantes();
      //primera vez 
      if(!this.estaEditando){
        this.ngOnInit();
      }
    }
  }

  getCantidadRepresentantes() {
    console.log(this.representantesForm.get( 'numRepresentantesRecorrido' ).valueChanges);
    this.representantesForm.get( 'numRepresentantesRecorrido' ).valueChanges
        .subscribe(
            value => {
              this.estaEditando = true;
              console.log("value: ", value, " - rep: ", this.representanteEtcrecorrido);
                    if ( this.representanteEtcrecorrido !== undefined && this.representanteEtcrecorrido.length > 0 ) {
                      this.representantes.clear();
                      for ( const representante of this.representanteEtcrecorrido ) {
                          this.representantes.push(
                              this.fb.group(
                                  {
                                      representanteEtcid: representante.representanteEtcid,
                                      proyectoEntregaEtcId: representante.proyectoEntregaEtcId,
                                      nombre: representante.nombre,
                                      cargo:  representante.cargo,
                                      dependencia: representante.dependencia,
                                      semaforo: representante.registroCompleto == true ? "completo" : 
                                                representante.registroCompleto == false && (representante.nombre == "" || representante.nombre == null || representante.cargo == "" || representante.cargo == null || representante.dependencia == "" || representante.dependencia == null) ? "en-proceso" : "sin-diligenciar"
                                  }
                              )
                          );
                      }
                      this.representantesForm.get( 'numRepresentantesRecorrido' ).setValidators( Validators.min( this.representantes.length ) );
                      const nuevosRepresentantes = Number( value ) - this.representantes.length;
                      if ( Number( value ) < this.representantes.length && Number( value ) > 0 ) {
                        this.openDialog(
                          '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                        );
                        this.representantesForm.get( 'numRepresentantesRecorrido' ).setValue( String( this.representantes.length ) );
                        return;
                      }
                      for ( let i = 0; i < nuevosRepresentantes; i++ ) {
                          this.representantes.push(
                              this.fb.group({
                                representanteEtcid: [null, Validators.required],
                                proyectoEntregaEtcId: [this.proyectoEntregaEtcId, Validators.required],  
                                nombre: [null, Validators.required],
                                cargo: [null, Validators.required],
                                dependencia: [null, Validators.required],
                                semaforo: "sin-diligenciar"
                              })
                          );
                      }
                    }
                    if (this.representanteEtcrecorrido !== undefined && this.representanteEtcrecorrido.length === 0 ){
                        if ( Number( value ) < 0 ) {
                            this.representantesForm.get( 'numRepresentantesRecorrido' ).setValue( '0' );
                        }
                        if ( Number( value ) > 0 ) {
                            if ( this.representantes.dirty === true ) {
                                this.representantesForm.get( 'numRepresentantesRecorrido' ).setValidators( Validators.min( this.representantes.length ) );
                                const nuevosRepresentantes = Number( value ) - this.representantes.length;
                                if ( Number( value ) < this.representantes.length && Number( value ) > 0 ) {
                                  this.openDialog(
                                    '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                                  );
                                  this.representantesForm.get( 'numRepresentantesRecorrido' ).setValue( String( this.representantes.length ) );
                                  return;
                                }
                                for ( let i = 0; i < nuevosRepresentantes; i++ ) {
                                    this.representantes.push(
                                        this.fb.group({
                                            representanteEtcid: [null, Validators.required],
                                            proyectoEntregaEtcId: [this.proyectoEntregaEtcId, Validators.required],  
                                            nombre: [null, Validators.required],
                                            cargo: [null, Validators.required],
                                            dependencia: [null, Validators.required],
                                            semaforo: "sin-diligenciar"
                                        })
                                    );
                                }
                            } else {
                                this.representantes.clear();
                                for ( let i = 0; i < Number( value ); i++ ) {
                                    this.representantes.push(
                                        this.fb.group({                                      
                                          representanteEtcid: [null, Validators.required],
                                          proyectoEntregaEtcId: [this.proyectoEntregaEtcId, Validators.required],  
                                          nombre: [null, Validators.required],
                                          cargo: [null, Validators.required],
                                          dependencia: [null, Validators.required],
                                          semaforo: "sin-diligenciar"
                                        })
                                    );
                                }
                            }
                        }
                    }
                    if ( this.representanteEtcrecorrido === undefined ) {
                        if ( Number( value ) < 0 ) {
                            this.representantesForm.get( 'numRepresentantesRecorrido' ).setValue( '0' );
                        }
                        if ( Number( value ) > 0 ) {
                            if ( this.representantes.dirty === true ) {
                                this.representantesForm.get( 'numRepresentantesRecorrido' )
                                .setValidators( Validators.min( this.representantes.length ) );
                                const nuevosRepresentantes = Number( value ) - this.representantes.length;
                                if ( Number( value ) < this.representantes.length && Number( value ) > 0 ) {
                                  this.openDialog( '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>' );
                                  this.representantesForm.get( 'numRepresentantesRecorrido' ).setValue( String( this.representantes.length ) );
                                  return;
                                }
                                for ( let i = 0; i < nuevosRepresentantes; i++ ) {
                                    this.representantes.push(
                                        this.fb.group({
                                          representanteEtcid: [null, Validators.required],
                                          proyectoEntregaEtcId: [this.proyectoEntregaEtcId, Validators.required],  
                                          nombre: [null, Validators.required],
                                          cargo: [null, Validators.required],
                                          dependencia: [null, Validators.required],
                                          semaforo: "sin-diligenciar"
                                        })
                                    );
                                }
                            } else {
                                this.representantes.clear();
                                for ( let i = 0; i < Number( value ); i++ ) {
                                    this.representantes.push(
                                        this.fb.group({
                                          representanteEtcid: [null, Validators.required],
                                          proyectoEntregaEtcId: [this.proyectoEntregaEtcId, Validators.required],  
                                          nombre: [null, Validators.required],
                                          cargo: [null, Validators.required],
                                          dependencia: [null, Validators.required],
                                          semaforo: "sin-diligenciar"
                                        })
                                    );
                                }
                            }
                        }
                  }
                }
        );
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    this.estaEditando = true;
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

  /*get representantes() {
    return this.formGestionCalidad.get( 'ensayosLaboratorio' ) as FormArray;
}*/

    arrayOne(n: number): any[] {
      return Array(n);
    }

}
