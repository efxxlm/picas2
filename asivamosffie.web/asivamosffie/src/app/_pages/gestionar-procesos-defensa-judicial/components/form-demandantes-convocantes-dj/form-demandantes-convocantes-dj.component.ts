import { Component, Input, OnInit, AfterViewInit, EventEmitter, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService, Respuesta } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial, DefensaJudicialService, DemandadoConvocado, DemandanteConvocante } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-demandantes-convocantes-dj',
  templateUrl: './form-demandantes-convocantes-dj.component.html',
  styleUrls: ['./form-demandantes-convocantes-dj.component.scss']
})
export class FormDemandantesConvocantesDjComponent implements OnInit {
  @Output() tieneDemanda = new EventEmitter();
  addressForm = this.fb.group({
    demandaContraFFIE: [null, Validators.required]
  });
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

  textoConvocantes = "demandante";
  textoConvocantesCapital = "Demandante";
  demandado_class: number = 0;
  convocado_class: number = 0;
  estaEditando = false;

  constructor(private fb: FormBuilder, public commonService: CommonService,
    public defensaService: DefensaJudicialService,
    public dialog: MatDialog, private router: Router) {
    this.crearFormulario();
    this.getNumeroContratos();
  }

  @Input() legitimacion: boolean;
  @Input() tipoProceso: string;
  @Input() defensaJudicial: DefensaJudicial;
  ngAfterViewInit() {
    this.cargarRegistro();
  }
  cargarRegistro() {


    /*      let defContraProyecto:DemandanteConvocante[]=[];
        for(let perfil of this.perfiles.controls){
          defContraProyecto.push({
            nombre:perfil.get("nomConvocado").value,
            tipoIdentificacionCodigo:perfil.get("tipoIdentificacion").value,
            numeroIdentificacion:perfil.get("numIdentificacion").value,
            direccion:perfil.get("direccion").value,
            email:perfil.get("correo").value,
            esConvocante:false
          });
        };
    */
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    this.formContratista.markAllAsTouched();
    if (this.defensaJudicial.esDemandaFfie == false) {
      this.textoConvocantes = "convocante";
      this.textoConvocantesCapital = "Convocante";
      this.tieneDemanda.emit('2');
    }
    if (this.defensaJudicial.esDemandaFfie == true) {
      this.tieneDemanda.emit('1');
    }
    this.addressForm.get("demandaContraFFIE").setValue(this.defensaJudicial.esDemandaFfie);
    this.formContratista.get("numeroContratos").setValue(this.defensaJudicial.numeroDemandantes);
    let i = 0;
    this.demandado_class = this.estaIncompletoDemandado(this.defensaJudicial);
    this.convocado_class = this.estaIncompletoConvocado(this.defensaJudicial);
    this.defensaJudicial.demandanteConvocante.forEach(element => {
      if (this.perfiles.controls[i]) {
        this.perfiles.controls[i].markAllAsTouched();
        this.perfiles.controls[i].get("demandanteConvocadoId").setValue(element.demandanteConvocadoId);
        this.perfiles.controls[i].get("nomConvocado").setValue(element.nombre);
        this.perfiles.controls[i].get("tipoIdentificacion").setValue(element.tipoIdentificacionCodigo);
        this.perfiles.controls[i].get("numIdentificacion").setValue(element.numeroIdentificacion);
        this.perfiles.controls[i].get("direccion").setValue(element.direccion);
        this.perfiles.controls[i].get("correo").setValue(element.email);
        //this.perfiles.controls[i].get("registroCompleto").setValue(element.registroCompleto);
        if (element.registroCompleto == null
          || (!element.registroCompleto
            && (element.nombre == null || element.nombre == '')
            && (element.tipoIdentificacionCodigo == null || element.tipoIdentificacionCodigo == '')
            && (element.numeroIdentificacion == null || element.numeroIdentificacion == '')
            && (element.direccion == null || element.direccion == '')
            && (element.email == null || element.email == '')
          )) {
          this.perfiles.controls[i].get("registroCompleto").setValue(null);
        } else if (!element.registroCompleto) {
          this.perfiles.controls[i].get("registroCompleto").setValue(false);
        } else if (element.registroCompleto) {
          this.perfiles.controls[i].get("registroCompleto").setValue(true);
        }
      }
      i++;
    });
    //defensaJudicial.eDemandaFFIE=this.addressForm.get("demandaContraFFIE").value;
    //=;
  }

  ngOnInit(): void {
    this.commonService.listaTipodocumento().subscribe(response => {
      this.tiposIdentificacionArray = response;
    });
    this.formContratista.get('numeroContratos').valueChanges
    .subscribe(value => {
      console.log(this.perfiles.length);
      console.log(value);
      if (this.perfiles.length < Number(value)) {
        for (let i = this.perfiles.length; i < Number(value); i++) {
          this.perfiles.push(
            this.fb.group(
              {
                demandanteConvocadoId: [null],
                nomConvocado: [null],
                tipoIdentificacion: [null],
                numIdentificacion: [null],
                direccion: [null],
                correo: [null],
                registroCompleto: [null],
              }
            )
          )
        }
      }
    });
  };


  getNumeroContratos() {
    this.formContratista.get( 'numeroContratos' ).valueChanges
        .subscribe(
            value => {
                if ( this.defensaJudicial !== undefined && this.defensaJudicial.demandanteConvocante.length > 0 ) {
                    this.perfiles.clear();
                    for (const demandanteConvocante of this.defensaJudicial.demandanteConvocante ) {
                        this.perfiles.push(
                            this.fb.group(
                                {
                                    demandanteConvocadoId:  demandanteConvocante.demandanteConvocadoId !== undefined ? demandanteConvocante.demandanteConvocadoId : null,
                                    nomConvocado:  demandanteConvocante.nombre !== undefined ? demandanteConvocante.nombre : null,
                                    tipoIdentificacion:  demandanteConvocante.tipoIdentificacionCodigo !== undefined ? demandanteConvocante.tipoIdentificacionCodigo : null,
                                    numIdentificacion:  demandanteConvocante.numeroIdentificacion !== undefined ? demandanteConvocante.numeroIdentificacion : null,
                                    direccion:  demandanteConvocante.direccion !== undefined ? demandanteConvocante.direccion : null,
                                    correo:  demandanteConvocante.email !== undefined ? demandanteConvocante.email : null,
                                    registroCompleto:  demandanteConvocante.registroCompleto !== undefined ? demandanteConvocante.registroCompleto : null,
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
                              demandanteConvocadoId: [null],
                              nomConvocado: [null],
                              tipoIdentificacion: [null],
                              numIdentificacion: [null],
                              direccion: [null],
                              correo: [null],
                              registroCompleto: [null],
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
                                      demandanteConvocadoId: [null],
                                      nomConvocado: [null],
                                      tipoIdentificacion: [null],
                                      numIdentificacion: [null],
                                      direccion: [null],
                                      correo: [null],
                                      registroCompleto: [null],
                                    })
                                );
                            }
                        } else {
                            this.perfiles.clear();
                            for ( let i = 0; i < Number( value ); i++ ) {
                                this.perfiles.push(
                                    this.fb.group({
                                      demandanteConvocadoId: [null],
                                      nomConvocado: [null],
                                      tipoIdentificacion: [null],
                                      numIdentificacion: [null],
                                      direccion: [null],
                                      correo: [null],
                                      registroCompleto: [null],
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
                                      demandanteConvocadoId: [null],
                                      nomConvocado: [null],
                                      tipoIdentificacion: [null],
                                      numIdentificacion: [null],
                                      direccion: [null],
                                      correo: [null],
                                      registroCompleto: [null],
                                    })
                                );
                            }
                        } else {
                            this.perfiles.clear();
                            for ( let i = 0; i < Number( value ); i++ ) {
                                this.perfiles.push(
                                    this.fb.group({
                                      demandanteConvocadoId: [null],
                                      nomConvocado: [null],
                                      tipoIdentificacion: [null],
                                      numIdentificacion: [null],
                                      direccion: [null],
                                      correo: [null],
                                      registroCompleto: [null],
                                    })
                                );
                            }
                        }
                    }
                }
            }
        );
  }

  get perfiles() {
    return this.formContratista.get('perfiles') as FormArray;
  };

  get numeroRadicado() {
    let numero;
    Object.values(this.formContratista.controls).forEach(control => {
      if (control instanceof FormArray) {
        Object.values(control.controls).forEach(control => {
          numero = control.get('numeroRadicadoFfieAprobacionCv') as FormArray;
        })
      }
    })
    return numero;
  };
  textoLimpio(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  textoLimpioMessage(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  crearFormulario() {
    this.formContratista = this.fb.group({
      numeroContratos: [''],
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
  
  eliminarPerfil( demandanteConvocadoId: number, numeroPerfil: number ) {
    this.openDialogSiNo( '', '¿Está seguro de eliminar esta información?' )
      .subscribe( value => {
        if ( value === true ) {
            if ( demandanteConvocadoId === 0 || demandanteConvocadoId == null) {
                this.perfiles.removeAt( numeroPerfil );
                this.formContratista.patchValue({
                  numeroContratos: `${ this.perfiles.length }`
                });
                this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
            } else {
              this.perfiles.removeAt( numeroPerfil );
              this.formContratista.patchValue({
                numeroContratos: `${ this.perfiles.length }`
              });
                this.defensaService.deleteDemandanteConvocante( demandanteConvocadoId , this.perfiles.length)
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

  agregarNumeroRadicado() {
    this.numeroRadicado.push(this.fb.control(''))
  }

  eliminarNumeroRadicado(numeroRadicado: number) {
    this.numeroRadicado.removeAt(numeroRadicado);
  };

  guardar() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    this.formContratista.markAllAsTouched();
    this.perfiles.markAllAsTouched();
    let defContraProyecto: DemandanteConvocante[] = [];
    for (let perfil of this.perfiles.controls) {
      defContraProyecto.push({
        demandanteConvocadoId: perfil.get("demandanteConvocadoId").value,
        nombre: perfil.get("nomConvocado").value,
        tipoIdentificacionCodigo: perfil.get("tipoIdentificacion").value,
        numeroIdentificacion: perfil.get("numIdentificacion").value,
        direccion: perfil.get("direccion").value,
        email: perfil.get("correo").value,
        esConvocante: false
      });
    };

    let defensaJudicial = this.defensaJudicial;
    if (!this.defensaJudicial.defensaJudicialId || this.defensaJudicial.defensaJudicialId == 0) {
      defensaJudicial = {
        defensaJudicialId: this.defensaJudicial.defensaJudicialId,
        tipoProcesoCodigo: this.tipoProceso,
        esLegitimacionActiva: this.legitimacion,
      };
    } else {
      this.tipoProceso != null ? defensaJudicial.tipoProcesoCodigo = this.tipoProceso : this.defensaJudicial.tipoProcesoCodigo;
      this.legitimacion != null ? defensaJudicial.esLegitimacionActiva = this.legitimacion : this.defensaJudicial.esLegitimacionActiva;
    }
    defensaJudicial.esDemandaFfie = this.addressForm.get("demandaContraFFIE").value;
    defensaJudicial.numeroDemandantes = this.formContratista.get("numeroContratos").value;

    defensaJudicial.demandanteConvocante = defContraProyecto;
    if (this.tipoProceso == null || this.legitimacion == null) {
      this.openDialog('', '<b>Falta registrar información.</b>');
    }
    else {
      this.defensaService.CreateOrEditDefensaJudicial(defensaJudicial)
        .subscribe((response: Respuesta) => {
          this.openDialog('', `<b>${response.message}</b>`, true, response.data ? response.data.defensaJudicialId : 0);
        },
          err => {
            this.openDialog('', err.message);
          });
    }
  }

  openDialog(modalTitle: string, modalText: string, redirect?: boolean, id?: number) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if (redirect) {
      dialogRef.afterClosed().subscribe(result => {
        if (id > 0 && this.defensaJudicial.defensaJudicialId != id) {
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(
            () => this.router.navigate(
              [
                '/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial',
                id
              ]
            )
          );
        }
        else {
          if (this.defensaJudicial.defensaJudicialId == id) {
            this.router.navigateByUrl('/', { skipLocationChange: true }).then(
              () => this.router.navigate(
                [
                  '/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial',
                  id
                ]
              )
            );
          } else {
            this.router.navigate(["/gestionarProcesoDefensaJudicial"], {});
          }
        }
      });
    }

  }

  cambioTipoTexto() {
    this.textoConvocantes = this.addressForm.value.demandaContraFFIE ? "convocante" : "demandante";
    this.textoConvocantesCapital = this.addressForm.value.demandaContraFFIE ? "Convocante" : "Demandante";
    if(this.estaEditando == true){
      if(this.addressForm.value.demandaContraFFIE==true){
        this.tieneDemanda.emit('2');
      }
      if(this.addressForm.value.demandaContraFFIE==false){
        this.tieneDemanda.emit('1');
      }
    }
  }

  estaIncompletoDemandado(defensaJudicial: DefensaJudicial): number {
    let retorno: number = 0;
    //sin-diligenciar:retorno===0,'en-proceso':retorno===1,'completo':retorno===2
    if (defensaJudicial != null) {
      let num_enproceso: number = 0;
      let num_sindiligenciar: number = 0;

      let num_convocados = defensaJudicial.numeroDemandantes;// total de convocados
      let num_completo = 0; //almacena los registros que estan completos
      defensaJudicial.demandadoConvocado.forEach(element => {
        if (element.esDemandado) {
          if (element.registroCompleto == null
            || (!element.registroCompleto
              && (element.nombre == null || element.nombre == '')
              && (element.tipoIdentificacionCodigo == null || element.tipoIdentificacionCodigo == '')
              && (element.numeroIdentificacion == null || element.numeroIdentificacion == '')
              && (element.direccion == null || element.direccion == '')
              && (element.email == null || element.email == '')
            )) {
            num_sindiligenciar = num_sindiligenciar + 1;
          } else if (!element.registroCompleto) {
            num_enproceso = num_enproceso + 1;
          } else if (element.registroCompleto) {
            num_completo = num_completo + 1;
          }
        }
      });
      if (num_sindiligenciar >= num_convocados) {
        retorno = 0;
      } else if (num_enproceso > 0 || (num_completo > 0 && num_completo < num_convocados)) {
        retorno = 1;
      } else if (num_completo >= num_convocados) {
        retorno = 2;
      }

    }

    return retorno;
  }

  estaIncompletoConvocado(defensaJudicial: DefensaJudicial): number {
    let retorno: number = 0;
    //sin-diligenciar:retorno===0,'en-proceso':retorno===1,'completo':retorno===2
    if (defensaJudicial != null) {
      let num_enproceso: number = 0;
      let num_sindiligenciar: number = 0;

      let num_convocados = defensaJudicial.numeroDemandados;// total de convocados
      let num_completo = 0; //almacena los registros que estan completos
      defensaJudicial.demandadoConvocado.forEach(element => {
        if (element.esConvocado) {
          if (element.registroCompleto == null
            || (!element.registroCompleto
              && (element.nombre == null || element.nombre == '')
              && (element.tipoIdentificacionCodigo == null || element.tipoIdentificacionCodigo == '')
              && (element.numeroIdentificacion == null || element.numeroIdentificacion == '')
              && (element.direccion == null || element.direccion == '')
              && (element.email == null || element.email == '')
              && (element.existeConocimiento == null)
            )) {
            num_sindiligenciar = num_sindiligenciar + 1;
          } else if (!element.registroCompleto) {
            num_enproceso = num_enproceso + 1;
          } else if (element.registroCompleto) {
            num_completo = num_completo + 1;
          }
        }
      });
      if (num_sindiligenciar >= num_convocados) {
        retorno = 0;
      } else if (num_enproceso > 0 || (num_completo > 0 && num_completo < num_convocados)) {
        retorno = 1;
      } else if (num_completo >= num_convocados) {
        retorno = 2;
      }
    }

    return retorno;
  }
}
