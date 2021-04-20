import { Component, Input, OnInit,AfterViewInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial, DefensaJudicialService, DemandadoConvocado } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-demandados-dj',
  templateUrl: './form-demandados-dj.component.html',
  styleUrls: ['./form-demandados-dj.component.scss']
})
export class FormDemandadosDjComponent implements OnInit {
  formContratista: FormGroup;
  editorStyle = {
    height: '45px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  tiposIdentificacionArray = [
  ];

  constructor ( private fb: FormBuilder,public commonService:CommonService,
    public defensaService:DefensaJudicialService,
    public dialog: MatDialog, private router: Router  ) {
    this.crearFormulario();
    this.getNumeroContratos();
  }

  @Input() legitimacion:boolean;
  @Input() tipoProceso:string;
  @Input() defensaJudicial:DefensaJudicial;

  estaEditando = false;
  ngAfterViewInit(){
    this.cargarRegistro();
  }

  cargarRegistro() {
    this.estaEditando = true;
    this.formContratista.markAllAsTouched();
    
      this.formContratista.get("numeroContratos").setValue(this.defensaJudicial.numeroDemandados);
      let i=0;  
      
      let listaDemandado:DemandadoConvocado[]= [];

      this.defensaJudicial.demandadoConvocado.forEach(element => {
        if (element.esDemandado == true)
        listaDemandado.push(element);
      });

      listaDemandado.forEach(element => {
          this.perfiles.controls[i].markAllAsTouched();
          this.perfiles.controls[i].get("demandadoConvocadoId").setValue(element.demandadoConvocadoId);
          this.perfiles.controls[i].get("nomConvocado").setValue(element.nombre);
          this.perfiles.controls[i].get("tipoIdentificacion").setValue(element.tipoIdentificacionCodigo);
          this.perfiles.controls[i].get("numIdentificacion").setValue(element.numeroIdentificacion);
          this.perfiles.controls[i].get("registroCompleto").setValue(element.registroCompleto);
        i++;
      });
  }

  ngOnInit(): void {
    this.commonService.listaTipodocumento().subscribe(response=>{
      this.tiposIdentificacionArray=response;
    });
  };

  getNumeroContratos() {
    this.formContratista.get( 'numeroContratos' ).valueChanges
        .subscribe(
            value => {
                if ( this.defensaJudicial !== undefined && this.defensaJudicial.demandadoConvocado.length > 0 ) {
                    this.perfiles.clear();
                    for (const demandadoConvocado of this.defensaJudicial.demandadoConvocado ) {
                        this.perfiles.push(
                            this.fb.group(
                                {
                                    demandadoConvocadoId:  demandadoConvocado.demandadoConvocadoId !== undefined ? demandadoConvocado.demandadoConvocadoId : null,
                                    nomConvocado:  demandadoConvocado.nombre !== undefined ? demandadoConvocado.nombre : null,
                                    tipoIdentificacion:  demandadoConvocado.tipoIdentificacionCodigo !== undefined ? demandadoConvocado.tipoIdentificacionCodigo : null,
                                    numIdentificacion:  demandadoConvocado.numeroIdentificacion !== undefined ? demandadoConvocado.numeroIdentificacion : null,
                                    registroCompleto:  demandadoConvocado.registroCompleto !== undefined ? demandadoConvocado.registroCompleto : null,
                                  }
                            )
                        );
                    }
                    this.formContratista.get( 'numeroContratos' ).setValidators( Validators.min( this.perfiles.length ) );
                    const nuevosConvocados = Number( value ) - this.perfiles.length;
                    if ( Number( value ) < this.perfiles.length && Number( value ) > 0 ) {
                      console.log("1");
                      this.openDialog(
                        '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                      );
                      this.formContratista.get( 'numeroContratos' ).setValue( String( this.perfiles.length ) );
                      return;
                    }
                    for ( let i = 0; i < nuevosConvocados; i++ ) {
                        this.perfiles.push(
                            this.fb.group({
                              demandadoConvocadoId: [ null ],
                              nomConvocado: [ null ],
                              tipoIdentificacion: [ null ],
                              numIdentificacion: [ null ],
                              registroCompleto: [ null ]
                            })
                        );
                    }
                }else if (this.defensaJudicial !== undefined && this.defensaJudicial.demandadoConvocado.length === 0 )
                {
                    if ( Number( value ) < 0 ) {
                        this.formContratista.get( 'numeroContratos' ).setValue( '0' );
                    }
                    if ( Number( value ) > 0 ) {
                        if ( this.formContratista.dirty === true ) {
                            this.formContratista.get( 'numeroContratos' )
                            .setValidators( Validators.min( this.perfiles.length ) );
                            const nuevosConvocados = Number( value ) - this.perfiles.length;
                            if ( Number( value ) < this.perfiles.length && Number( value ) > 0 ) {
                              console.log("2");
                              this.openDialog(
                                '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                              );
                              this.formContratista.get( 'numeroContratos' ).setValue( String( this.perfiles.length ) );
                              return;
                            }
                            for ( let i = 0; i < nuevosConvocados; i++ ) {
                                this.perfiles.push(
                                    this.fb.group({
                                      demandadoConvocadoId: [ null ],
                                      nomConvocado: [ null ],
                                      tipoIdentificacion: [ null ],
                                      numIdentificacion: [ null ],
                                      registroCompleto: [ null ]
                                    })
                                );
                            }
                        } else {
                            this.perfiles.clear();
                            for ( let i = 0; i < Number( value ); i++ ) {
                                this.perfiles.push(
                                    this.fb.group({
                                      demandadoConvocadoId: [ null ],
                                      nomConvocado: [ null ],
                                      tipoIdentificacion: [ null ],
                                      numIdentificacion: [ null ],
                                      registroCompleto: [ null ]
                                    })
                                );
                            }
                        }
                    }
                }else if ( this.defensaJudicial === undefined ) {
                    if ( Number( value ) < 0 ) {
                        this.formContratista.get( 'numeroContratos' ).setValue( '0' );
                    }
                    if ( Number( value ) > 0 ) {
                        if ( this.perfiles.dirty === true ) {
                            this.formContratista.get( 'numeroContratos' )
                            .setValidators( Validators.min( this.perfiles.length ) );
                            const nuevosConvocados = Number( value ) - this.perfiles.length;
                            if ( Number( value ) < this.perfiles.length && Number( value ) > 0 ) {
                              console.log("3");
                              this.openDialog( '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>' );
                              this.formContratista.get( 'numeroContratos' ).setValue( String( this.perfiles.length ) );
                              return;
                            }
                            for ( let i = 0; i < nuevosConvocados; i++ ) {
                                this.perfiles.push(
                                    this.fb.group({
                                      demandadoConvocadoId: [ null ],
                                      nomConvocado: [ null ],
                                      tipoIdentificacion: [ null ],
                                      numIdentificacion: [ null ],
                                      registroCompleto: [ null ]
                                    })
                                );
                            }
                        } else {
                            this.perfiles.clear();
                            for ( let i = 0; i < Number( value ); i++ ) {
                                this.perfiles.push(
                                    this.fb.group({
                                      demandadoConvocadoId: [ null ],
                                      nomConvocado: [ null ],
                                      tipoIdentificacion: [ null ],
                                      numIdentificacion: [ null ],
                                      registroCompleto: [ null ]
                                    })
                                );
                            }
                        }
                    }
                }
            }
        );
  }


	  get perfiles () {
		return this.formContratista.get( 'perfiles' ) as FormArray;
	  };

  get numeroRadicado () {
    let numero;
    Object.values( this.formContratista.controls ).forEach( control => {
      if ( control instanceof FormArray ) {
        Object.values( control.controls ).forEach( control => {
          numero = control.get( 'numeroRadicadoFfieAprobacionCv' ) as FormArray;
        } )
      }
    } )
    return numero;
  };
  textoLimpio (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  maxLength (e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  crearFormulario () {
    this.formContratista = this.fb.group({
      numeroContratos: [ '' ],
      perfiles: this.fb.array([])
    });
  };

  openDialogSiNo(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    return dialogRef.afterClosed();
  }

  eliminarPerfil( demandadoConvocadoId: number, numeroPerfil: number ) {
    this.openDialogSiNo( '', '¿Está seguro de eliminar esta información?' )
      .subscribe( value => {
        if ( value === true ) {
            if ( demandadoConvocadoId === 0 || demandadoConvocadoId == null) {
                this.perfiles.removeAt( numeroPerfil );
                this.formContratista.patchValue({
                  numeroContratos: `${ this.perfiles.length }`
                });
                this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
            } else {
              this.perfiles.removeAt( numeroPerfil );
                this.defensaService.deleteDemandadoConvocado( demandadoConvocadoId ,this.perfiles.length)
                    .subscribe(
                        response => {
                            this.openDialog( '', `<b>${ response.message }</b>` );
                            this.router.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                () =>   this.router.navigate(
                                            [
                                                '/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial',
                                                this.defensaJudicial.defensaJudicialId
                                            ]
                                        )
                            );
                        },
                        err => this.openDialog( '', `<b>${ err.message }</b>` )
                    );
            }
        }
      } );
}
  agregarNumeroRadicado () {
    this.numeroRadicado.push( this.fb.control( '' ) )
  }

  eliminarNumeroRadicado ( numeroRadicado: number ) {
    this.numeroRadicado.removeAt( numeroRadicado );
  };

  guardar () {
    this.estaEditando = true;
    this.formContratista.markAllAsTouched();
    this.perfiles.markAllAsTouched();
    // console.log( this.formContratista );
    let defContraProyecto:DemandadoConvocado[]=[];
    for(let perfil of this.perfiles.controls){
      defContraProyecto.push({
        demandadoConvocadoId:perfil.get("demandadoConvocadoId").value,
        nombre:perfil.get("nomConvocado").value,
        tipoIdentificacionCodigo:perfil.get("tipoIdentificacion").value,
        numeroIdentificacion:perfil.get("numIdentificacion").value,        
        esConvocado:false, //para este modulo, no lo es
        esDemandado:true //nuevo campo - diferenciar entre demandado y convocado
      });
    };
    
    let defensaJudicial=this.defensaJudicial;
    if(!this.defensaJudicial.defensaJudicialId||this.defensaJudicial.defensaJudicialId==0)
    {
      defensaJudicial={
        defensaJudicialId:this.defensaJudicial.defensaJudicialId,
        //legitimacionCodigo:this.legitimacion,
        tipoProcesoCodigo:this.tipoProceso,
        //cantContratos:this.formContratista.get( 'numeroContratos' ).value,
        esLegitimacionActiva:this.legitimacion,
        esCompleto:false,      
      };
    }else{
      this.tipoProceso != null ? defensaJudicial.tipoProcesoCodigo = this.tipoProceso : this.defensaJudicial.tipoProcesoCodigo;
      this.legitimacion != null ? defensaJudicial.esLegitimacionActiva = this.legitimacion : this.defensaJudicial.esLegitimacionActiva;
    }
    defensaJudicial.numeroDemandados=this.formContratista.get("numeroContratos").value;
    defensaJudicial.demandadoConvocado=defContraProyecto;
      console.log(defensaJudicial);
      this.defensaService.CreateOrEditDefensaJudicial(defensaJudicial).subscribe(
        response=>{
          this.openDialog('', `<b>${response.message}</b>`,true,response.data?response.data.defensaJudicialId:0);
        }
      );
  }

  openDialog(modalTitle: string, modalText: string,redirect?:boolean,id?:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if (redirect) {
      dialogRef.afterClosed().subscribe(result => {
        if (id > 0 && this.defensaJudicial.defensaJudicialId != id) {
          this.router.navigateByUrl( '/', {skipLocationChange: true} ).then(
            () =>   this.router.navigate(
                        [
                            '/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial',
                            id
                        ]
                    )
          );
        }
        else {
          if(this.defensaJudicial.defensaJudicialId == id){
            this.router.navigateByUrl( '/', {skipLocationChange: true} ).then(
              () =>   this.router.navigate(
                          [
                              '/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial',
                              id
                          ]
                      )
            );
          }else{
            this.router.navigate(["/gestionarProcesoDefensaJudicial"], {});
          }
        }
      });
    }
  }

}
