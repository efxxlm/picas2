import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormGroup, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectChange } from '@angular/material/select';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { CofinanciacionAportante, CofinanciacionService } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { CommonService, Dominio, Localizacion, TiposAportante } from 'src/app/core/_services/common/common.service';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ComponenteAportanteNovedad, ComponenteFuenteNovedad, ComponenteUsoNovedad, NovedadContractual, NovedadContractualAportante } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-form-detallar-solicitud-novedad',
  templateUrl: './form-detallar-solicitud-novedad.component.html',
  styleUrls: ['./form-detallar-solicitud-novedad.component.scss']
})
export class FormDetallarSolicitudNovedadComponent implements OnInit {

  
  @Input() novedad: NovedadContractual;
  @Output() guardar = new EventEmitter();
  tipoNovedadAdicion = '3';
  //idSolicitud: number;
  //municipio: string;
  //tipoIntervencion: string;
  presupuestoAdicionalSolicitado = 0;
  addressForm = this.fb.group([]);
  fasesSelect: Dominio[] = [];
  listaUsos: any[] = [];
  listaAportantes: any[] = [];
  //tipoAportantes: any[] = [];
  tipoAportantes: any[] = [];
  listaTipoAportante: Dominio[];
  listaComponentes: any[] = [];
  componentesSelect: Dominio[] = [];
  usosSelect: Dominio[] = [];
  realizoPeticion: boolean = false;
  esSaldoPermitido: boolean = false;
  listaFaseUsosComponentes: any[] = [];
  listaDepartamentos: Localizacion[] = [];
  listaMunicipios: Localizacion[] = [];
  estaEditando = false;

  listaVigencias: any[] = [];
  listaAportante: any[] = [];
  listaTotalNuevas: any[] = [];
  tipoAportante = TiposAportante;
  listadoDepto: any[] = [];
  listadoMun: any[] = [];
  listaNombreAportantes: any[] = [];

  createFormulario() {
    return this.fb.group({
      aportantes: this.fb.array([])
    });
  }

  /*private declararInputFile() {
    this.nuevoAportante = new FormControl(false);
  }*/

  componentes(i: number) {
    return this.aportantes.controls[i].get('componentes') as FormArray;
  }

  get aportantes() {
    return this.addressForm.get('aportantes') as FormArray;
  }

  usos(posicionAportante: number, posicionComponente: number, posicionFuente: number) {
    // const control = this.addressForm.get('componentes') as FormArray;
    // return control.controls[i].get('usos') as FormArray;
    const fuentes = this.componentes(posicionAportante).controls[posicionComponente].get('fuentes') as FormArray;
    //console.log( fuentes, posicionFuente )
    const usos = fuentes.controls[posicionFuente].get('usos') as FormArray;
    return usos;
  }

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private contractualNoveltyService: ContractualNoveltyService,
    private projectContractingService: ProjectContractingService,
    private commonService: CommonService,
    private cofinancingService: CofinanciacionService,
    public dialog: MatDialog,
    private router: Router,

  ) {
  }

  openDialogConfirmar(modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    confirmarDialog.afterClosed()
      .subscribe(response => {
        if (response === true) {
          this.onSubmit();
        }
      });
  };

  createAportante() {

    return this.fb.group({
      listaFuentes: [],
      nombreAportante: [],
      estadoSemaforo: [null],
      saldoDisponible: [null],
      contratacionProyectoAportanteId: [],
      cofinanciacionAportanteId: [],
      tipoAportante: [],
      depaetamento: [],
      municipio: [],
      nuevoAportante: false,
      proyectoAportanteId: [],
      valorAportanteProyecto: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)]),
      ],
      componentes: this.fb.array([])
    });
  };

  ngOnInit(): void {

    if ( this.novedad.novedadContractualDescripcion !== undefined ) {
      if ( this.novedad.novedadContractualDescripcion.length > 0 ) {
        const novedadAdicion = this.novedad.novedadContractualDescripcion.find( novedadAdicion => novedadAdicion.tipoNovedadCodigo === this.tipoNovedadAdicion )

        if ( novedadAdicion !== undefined ) {
          if ( novedadAdicion.presupuestoAdicionalSolicitado !== undefined ) {
            this.presupuestoAdicionalSolicitado = novedadAdicion.presupuestoAdicionalSolicitado;
          }
        }
      }
    }

    this.addressForm = this.createFormulario();
    //this.declararInputFile();
    this.route.params.subscribe((params: Params) => {
      const id = params.id;

      forkJoin([
        this.commonService.listaFases(),
        this.commonService.listaComponentes(),
        this.commonService.listaUsos(),
        this.projectContractingService.getListFaseComponenteUso(),
        this.contractualNoveltyService.GetAportanteByContratacion(this.novedad.contrato.contratacionId),
        this.commonService.listaTipoAportante(),
        this.commonService.listaDepartamentos(),
        this.commonService.listaAportanteByTipoAportanteId(Number(this.tipoAportante.ET)),
        this.commonService.listaAportanteByTipoAportanteId(Number(this.tipoAportante.FFIE)),
        this.commonService.listaAportanteByTipoAportanteId(Number(this.tipoAportante.Tercero))
        //this.projectContractingService.getContratacionProyectoById(id),
      ])

        .subscribe(response => {

          this.fasesSelect = response[0];
          this.componentesSelect = response[1];
          this.usosSelect = response[2];
          this.listaFaseUsosComponentes = response[3];
          this.listaAportantes = response[4];
          //this.contratacionProyecto = response[5];
          this.tipoAportantes = response[5];
          this.listaTipoAportante = response[5];
          this.listaDepartamentos = response[6];
          this.listaTotalNuevas += response[7].filter(x => x.registroCompleto == true && !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId)  && !this.aportantes.controls.some(aportante => aportante.get('cofinanciacionAportanteId').value === x.cofinanciacionAportanteId));
          this.listaTotalNuevas += response[8].filter(x => x.registroCompleto == true && !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId)  && !this.aportantes.controls.some(aportante => aportante.get('cofinanciacionAportanteId').value === x.cofinanciacionAportanteId));
          this.listaTotalNuevas += response[9].filter(x => x.registroCompleto == true && !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId)  && !this.aportantes.controls.some(aportante => aportante.get('cofinanciacionAportanteId').value === x.cofinanciacionAportanteId));
          
          setTimeout(() => {

            if (this.componentesSelect.length > 0) {
              this.listaComponentes = this.componentesSelect.filter(value => this.novedad.contrato.contratacion.tipoSolicitudCodigo === value.codigo);
            };

            if (this.novedad.novedadContractualAportante === undefined || this.novedad.novedadContractualAportante.length === 0) {

              this.addAportante();

              console.log('aa')
            }

            this.novedad.novedadContractualAportante.forEach(apo => {

              console.log(apo)
              const grupoAportante = this.createAportante();
              const listaComponentes = grupoAportante.get('componentes') as FormArray;

              this.contractualNoveltyService.GetFuentesByAportante(apo.cofinanciacionAportanteId)
                .subscribe(respuesta => {
                  console.log(respuesta);
                  grupoAportante.get('listaFuentes').setValue(respuesta);

                  grupoAportante.get('contratacionProyectoAportanteId').setValue(apo.novedadContractualAportanteId);
                  grupoAportante.get('proyectoAportanteId').setValue(apo.cofinanciacionAportanteId);
                  if (apo.valorAporte !== 0) {
                    this.esSaldoPermitido = true;
                  }
                  grupoAportante.get('valorAportanteProyecto').setValue(apo.valorAporte);
                  grupoAportante.get('saldoDisponible').setValue(apo['saldoDisponible'] ? apo['saldoDisponible'] : 0);
                  grupoAportante.get('cofinanciacionAportanteId').setValue(apo.cofinanciacionAportanteId);
                  grupoAportante.get('nombreAportante').setValue(apo.nombreAportante);

                  if (apo.componenteAportanteNovedad.length > 0) {
                    apo.componenteAportanteNovedad.forEach(compoApo => {
                      const grupoComponente = this.createComponente();
                      const faseSeleccionada = this.fasesSelect.find(f => f.codigo == compoApo.faseCodigo);
                      const componenteSeleccionado = this.componentesSelect.find(c => c.codigo == compoApo.tipoComponenteCodigo);
                      const listaFuentes = grupoComponente.get('fuentes') as FormArray;

                      if (compoApo['registroCompleto'] !== undefined && compoApo['registroCompleto'] === true) {
                        grupoAportante.get('estadoSemaforo').setValue('completo')
                      };
                      if (compoApo['registroCompleto'] !== undefined && compoApo['registroCompleto'] === false) {
                        grupoAportante.get('estadoSemaforo').setValue('en-proceso')
                      };

                      let usos = this.listaFaseUsosComponentes.filter(p => p.faseId === faseSeleccionada?.codigo && p.componenteId === componenteSeleccionado?.codigo);
                      let listaDeUsos: Dominio[] = [];

                      if (this.usosSelect.length > 0) {
                        usos.forEach(u => {
                          listaDeUsos.push(this.usosSelect.find(uso => u.usoId == uso.codigo));
                        });

                      };


                      grupoComponente.get('componenteAportanteNovedadId').setValue(compoApo.componenteAportanteNovedadId);
                      grupoComponente.get('contratacionProyectoAportanteId').setValue(compoApo.novedadContractualAportanteId);
                      grupoComponente.get('fase').setValue(faseSeleccionada);
                      grupoComponente.get('componente').setValue(componenteSeleccionado);

                      //grupoComponente.get('listaUsos').setValue(listaDeUsos)

                      compoApo['componenteFuenteNovedad'].forEach(fuente => {
                        const grupoFuente = this.createFuente();
                        const listaUsos = grupoFuente.get('usos') as FormArray;

                        grupoFuente.get('componenteFuenteNovedadId').setValue(fuente.componenteFuenteNovedadId)
                        grupoFuente.get('listaUsos').setValue(listaDeUsos)
                        grupoFuente.get('fuenteId').setValue(fuente.fuenteFinanciacionId)

                        console.log(grupoFuente.get('fuenteId').value)
                        console.log(grupoAportante.get('listaFuentes').value)

                        fuente.componenteUsoNovedad.forEach(uso => {
                          const grupoUso = this.createUso();
                          const usoSeleccionado = this.usosSelect.find(u => u.codigo == uso.tipoUsoCodigo);

                          grupoUso.get('componenteUsoId').setValue(uso.componenteUsoNovedadId ? uso.componenteUsoNovedadId : 0);
                          grupoUso.get('componenteAportanteId').setValue(uso.componenteAportanteNovedadId);
                          grupoUso.get('usoDescripcion').setValue(usoSeleccionado);
                          grupoUso.get('valorUso').setValue(uso.valorUso);

                          if (grupoAportante.get('valorAportanteProyecto').value === 0 && grupoUso.get('valorUso').value === 0) {
                            grupoAportante.get('estadoSemaforo').setValue('sin-diligenciar');
                          }

                          listaUsos.push(grupoUso);
                        });
                        listaFuentes.push(grupoFuente)
                      });




                      listaComponentes.push(grupoComponente);
                    });
                  } else {

                    const grupoComponente = this.createComponente();
                    const grupoUso = this.createUso();
                    const listaUsos = grupoComponente.get('usos') as FormArray;

                    listaUsos.push(grupoUso);

                    listaComponentes.push(grupoComponente);
                  }
                  this.aportantes.push(grupoAportante);
                  console.log(this.aportantes.length , this.novedad.novedadContractualAportante.length);
                  if(this.aportantes.length > 0 && this.aportantes.length === this.novedad.novedadContractualAportante.length){
                    let i = 0;
                    this.aportantes.controls.forEach(aportante => {
                      if(!this.listaAportantes.some(e => e.cofinanciacionAportanteId === aportante.get('cofinanciacionAportanteId').value)){
                        aportante.get('nuevoAportante').setValue(true); 
                          let cofinanciacionAportante = this.novedad.novedadContractualAportante[i].cofinanciacionAportante;
                          if(cofinanciacionAportante != null){
                            aportante.get('tipoAportante').setValue(cofinanciacionAportante.tipoAportanteId); 
                            this.getAportanteById(cofinanciacionAportante.tipoAportanteId,i);
                            aportante.get('depaetamento').setValue(cofinanciacionAportante.departamentoId); 
                            aportante.get('municipio').setValue(cofinanciacionAportante.municipioId); 
                            aportante.get('nombreAportante').setValue(this.novedad.novedadContractualAportante[i].nombreAportante); 
                            aportante.get('cofinanciacionAportanteId').setValue(this.novedad.novedadContractualAportante[i].cofinanciacionAportanteId)
                          }
                      }else{
                        aportante.get('nuevoAportante').setValue(false); 
                      }
                      i++;
                    });
                  }
                  this.estaEditando = true;
                  this.addressForm.markAllAsTouched();
                });
            });

          }, 1000);
        });


    });

  }

  changeFase(posicionAportante, posicionComponente) {

    this.componentes(posicionAportante).controls[posicionComponente].get('componente').setValue(null);

    let posicionFuente = 0;
    this.fuentes(posicionAportante, posicionComponente).controls.forEach(fuente => {
      this.usos(posicionAportante, posicionComponente, posicionFuente).controls.forEach(control => {
        control.get('usoDescripcion').setValue(null);
        posicionFuente++;
      })
    });


  }

  getListaUsosFiltrado(posicionAportante, posicionComponente, posicionFuente, posicionUso) {


    let usoSeleccionado = this.usos(posicionAportante, posicionComponente, posicionFuente).controls[posicionUso];

    //let listaUsos =this.componentes(posicionAportante).controls[posicionComponente].get('listaUsos').value.map((x) => x);
    let listaUsos: any[] = [];


    if (this.fuentes(posicionAportante, posicionComponente).controls[posicionFuente].get('listaUsos').value)
      this.fuentes(posicionAportante, posicionComponente).controls[posicionFuente].get('listaUsos').value.forEach(u => {
        listaUsos.push(u)
      });

    this.usos(posicionAportante, posicionComponente, posicionFuente).controls.forEach(u => {

      if (listaUsos !== undefined && u.value.usoDescripcion)
        listaUsos = listaUsos.filter(uso => uso.codigo != u.value.usoDescripcion.codigo);

    });

    if (usoSeleccionado?.value?.usoDescripcion)
      listaUsos.push(usoSeleccionado?.value?.usoDescripcion)

    return listaUsos;
  }

  getlistaUsos(posicionAportante, posicionComponente) {

    if (posicionAportante !== undefined && posicionComponente !== undefined) {

      let fase = this.componentes(posicionAportante).controls[posicionComponente].get('fase').value;
      let componente = this.componentes(posicionAportante).controls[posicionComponente].get('componente').value;

      let usos = this.listaFaseUsosComponentes.filter(p => p.faseId === fase.codigo && p.componenteId === componente.codigo);
      if (this.usosSelect.length > 0) {
        let listaDeUsos: Dominio[] = [];
        usos.forEach(u => {
          listaDeUsos.push(this.usosSelect.find(uso => u.usoId == uso.codigo));
        });

        let fuentes = this.fuentes(posicionAportante, posicionComponente);
        fuentes.controls.forEach(f => {
          f.get('listaUsos').setValue(listaDeUsos)
        });
      };
    };
  };

  validarSaldoDisponible(saldoIngresado: number, nombreAportante: string, aportante) {
    let saldoDisponible = this.presupuestoAdicionalSolicitado;

     if (saldoIngresado > saldoDisponible) {
       this.openDialog('', `<b>El valor del aportante ${nombreAportante} al proyecto es superior al valor disponible, verifique por favor con él área financiera.</b>`);
       this.esSaldoPermitido = false;
     } else if (saldoIngresado <= saldoDisponible) {
        this.esSaldoPermitido = true;
     };
  };

  deleteUsoSeleccionado(usoCodigo: any) {
    this.listaUsos = this.listaUsos.filter(uso => uso.codigo !== usoCodigo);
  };


  addUso(posicionAportante: number, posicioncomponente: number, posicionFuente) {

    let fuentes = this.componentes(posicionAportante).controls[posicioncomponente].get('fuentes') as FormArray;
    if (fuentes.controls[posicionFuente].get('listaUsos').value.length === this.usos(posicionAportante, posicioncomponente, posicionFuente).controls.length) {
      this.openDialog('', `<b>No se encuentran usos disponibles para el componente de ${this.novedad.contrato.contratacion.tipoSolicitudCodigo === '2' ? 'Interventoria' : 'Obra'}.</b>`);
      return;
    };
    const listaUsos = fuentes.controls[posicionFuente].get('usos') as FormArray;
    listaUsos.push(this.createUso());
  }

  deleteUso(usos: any, i: number) {
    this.openDialogTrueFalse('', '<b>¿Está seguro de eliminar esta información?</b>')
    .subscribe(
      value => {
        if (value === true) {
          if (usos.at(i).get('componenteUsoId').value !== null) {
            this.contractualNoveltyService.eliminarComponenteUsoNovedad(usos.at(i).get('componenteUsoId').value)
              .subscribe(
                () => {
                  usos.removeAt(i);
                  this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
                },
                err => this.openDialog('', `<b>${err.message}</b>`)
              );
          } else {
            usos.removeAt(i);
            this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
          }
        }
      }
    );
  }
  

  createUso(): FormGroup {
    return this.fb.group({
      componenteUsoId: [],
      componenteAportanteId: [],
      usoDescripcion: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)])],
      valorUso: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)])
      ],
      listaUsos: []
    });
  }

  addAportante() {
    const grupoAportante = this.createAportante();
    const listaComponentes = grupoAportante.get('componentes') as FormArray;

    const grupoComponente = this.createComponente();
    const listaFuentes = grupoComponente.get('fuentes') as FormArray;

    const grupoFuente = this.createFuente()
    const listaUsos = grupoFuente.get('usos') as FormArray;

    const grupoUso = this.createUso();

    listaUsos.push(grupoUso);
    listaFuentes.push(grupoFuente)

    listaComponentes.push(grupoComponente);
    this.aportantes.push(grupoAportante);
    this.updateOldAportante();
  }

  addComponent(i: number) {


    let grupoComponente = this.createComponente();
    //let listaUsos = grupoComponente.get('usos') as FormArray;
    //listaUsos.push(this.createUso());
    let listaFuentes = grupoComponente.get('fuentes') as FormArray;
    let grupoUso = this.createUso();
    let grupofuente = this.createFuente();
    let listaUsos = grupofuente.get('usos') as FormArray;
    listaUsos.push(grupoUso);
    listaFuentes.push(grupofuente)
    this.componentes(i).push(grupoComponente);
  }

  createComponente(): FormGroup {

    return this.fb.group({
      componenteAportanteNovedadId: [],
      contratacionProyectoAportanteId: [],
      fase: [null, Validators.required],
      componente: [null, Validators.required],
      //usos: this.fb.array([]),
      //listaUsos: this.listaUsos,
      fuentes: this.fb.array([]),
    });
  }

  addFuente(posicionAportante: number, posicionComponente) {
    let grupofuente = this.createFuente();
    let listaUsos = grupofuente.get('usos') as FormArray;
    listaUsos.push(this.createUso());
    let fuentes = this.componentes(posicionAportante).controls[posicionComponente].get('fuentes') as FormArray;
    fuentes.controls.push(grupofuente);

    this.getlistaUsos(posicionAportante, posicionComponente)
  }

  deleteFuente(fuentes: any, i: number) {
    this.openDialogTrueFalse('', '<b>¿Está seguro de eliminar esta información?</b>')
    .subscribe(
      value => {
        if (value === true) {
          if (fuentes.at(i).get('componenteFuenteNovedadId').value !== null) {
            this.contractualNoveltyService.eliminarComponenteFuenteNovedad(fuentes.at(i).get('componenteFuenteNovedadId').value)
              .subscribe(
                () => {
                  fuentes.removeAt(i);
                  this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
                },
                err => this.openDialog('', `<b>${err.message}</b>`)
              );
          } else {
            fuentes.removeAt(i);
            this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
          }
        }
      }
    );
  }

  createFuente(): FormGroup {
    return this.fb.group({
      componenteFuenteNovedadId: [],
      usos: this.fb.array([]),
      listaUsos: this.listaUsos,
      fuenteId: [],
    });
  }

  fuentes(posicionAportante: number, PosicionComponente: number) {
    // const control = this.addressForm.get('componentes') as FormArray;
    // return control.controls[i].get('usos') as FormArray;
    return this.componentes(posicionAportante).controls[PosicionComponente].get('fuentes') as FormArray;
  }

  borrarArray(j: number, i: number) {
    this.openDialogTrueFalse('', '<b>¿Está seguro de eliminar esta información?</b>')
      .subscribe(
        value => {
          if (value === true) {
            if (this.componentes(j).at(i).get('componenteAportanteNovedadId').value !== null) {
              this.contractualNoveltyService.eliminarComponenteAportanteNovedad(this.componentes(j).at(i).get('componenteAportanteNovedadId').value)
                .subscribe(
                  () => {
                    this.componentes(j).removeAt(i);
                    this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
                  },
                  err => this.openDialog('', `<b>${err.message}</b>`)
                );
            } else {
              this.componentes(j).removeAt(i);
            }
          }
        }
      );
  }

  borrarAportante(posicionAportante: number) {
    this.openDialogTrueFalse('', '<b>¿Está seguro de eliminar esta información?</b>')
      .subscribe(
        value => {
          if (value === true) {
            if (this.novedad.novedadContractualAportante.length >= this.aportantes.length) {
              this.contractualNoveltyService.eliminarNovedadContractualAportante(this.novedad.novedadContractualAportante[posicionAportante].novedadContractualAportanteId)
                .subscribe(
                  () => {
                    this.aportantes.removeAt(posicionAportante);
                    this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
                  },
                  err => this.openDialog('', `<b>${err.message}</b>`)
                );
            } else {
              this.aportantes.removeAt(posicionAportante);
            }
          }
        }
      );
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  openDialogTrueFalse(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    return dialogRef.afterClosed()
  }

  changeAportante(posicionAportante) {
    let listaFuentes = this.aportantes.controls[posicionAportante].get('listaFuentes');
    let idAportante = this.aportantes.controls[posicionAportante].get('cofinanciacionAportanteId').value;


    this.contractualNoveltyService.GetFuentesByAportante(idAportante)
      .subscribe(respuesta => {
        listaFuentes.setValue(respuesta);
      });
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    let valoresCorrectos = true;
    let valorTotalSumado = 0;
    let totalAportantes = 0;
    let totalValorAportanteProyecto = 0;

    this.novedad.novedadContractualAportante = [];

    this.aportantes.controls.forEach( controlAportante => {
      if ( controlAportante.get( 'valorAportanteProyecto' ).value !== null ) {
        totalValorAportanteProyecto += controlAportante.get( 'valorAportanteProyecto' ).value
      }
    } )

    // aportantes
    this.aportantes.controls.forEach(controlAportante => {
      const listaComponentes = controlAportante.get('componentes') as FormArray;

      if (controlAportante.get('valorAportanteProyecto').value !== 0) {
        totalAportantes++;
      };

      valorTotalSumado += controlAportante.get('valorAportanteProyecto').value;

      const aportante: NovedadContractualAportante = {
        novedadContractualAportanteId: controlAportante.get('contratacionProyectoAportanteId').value,
        novedadContractualId: this.novedad.novedadContractualId,
        cofinanciacionAportanteId: controlAportante.get('cofinanciacionAportanteId').value,
        valorAporte: controlAportante.get('valorAportanteProyecto').value,
        componenteAportanteNovedad: []
      };

      let valorTotal = 0;
      let valorSumado = 0;

      valorTotal = aportante.valorAporte;

      // componentes
      listaComponentes.controls.forEach(controlComponente => {

        const listaFuentes = controlComponente.get('fuentes') as FormArray;

        const componenteAportante: ComponenteAportanteNovedad = {
          componenteAportanteNovedadId: controlComponente.get('componenteAportanteNovedadId').value,
          novedadContractualAportanteId: aportante.novedadContractualAportanteId,
          cofinanciacionAportanteId: aportante.cofinanciacionAportanteId,
          tipoComponenteCodigo: controlComponente.get('componente').value ? controlComponente.get('componente').value.codigo : null,
          faseCodigo: controlComponente.get('fase').value ? controlComponente.get('fase').value.codigo : null,
          componenteFuenteNovedad: []
        };

        listaFuentes.controls.forEach(controlFuentes => {
          const listaUsos = controlFuentes.get('usos') as FormArray;

          const componenteFuenteNovedad: ComponenteFuenteNovedad = {
            componenteAportanteNovedadId: componenteAportante.componenteAportanteNovedadId,
            componenteFuenteNovedadId: controlFuentes.get('componenteFuenteNovedadId').value,
            fuenteFinanciacionId: controlFuentes.get('fuenteId').value,
            componenteUsoNovedad: []
          }

          listaUsos.controls.forEach(controlUsos => {
            const componenteUso: ComponenteUsoNovedad = {
              componenteUsoNovedadId: controlUsos.get('componenteUsoId').value,
              componenteAportanteNovedadId: componenteAportante.componenteAportanteNovedadId,
              tipoUsoCodigo: controlUsos.get('usoDescripcion').value ? controlUsos.get('usoDescripcion').value.codigo : null,
              valorUso: controlUsos.get('valorUso').value,
            };

            valorSumado = valorSumado + componenteUso.valorUso;

            componenteFuenteNovedad.componenteUsoNovedad.push(componenteUso);
          });

          componenteAportante.componenteFuenteNovedad.push(componenteFuenteNovedad)
        });

        aportante.componenteAportanteNovedad.push(componenteAportante);

      });

      if (valorSumado != valorTotal) {

        valoresCorrectos = false;
      };

      this.novedad.novedadContractualAportante.push(aportante);
    });

    if ( totalValorAportanteProyecto !== this.presupuestoAdicionalSolicitado ) {
      this.openDialog('', '<b>No coincide el valor de los aportes con el presupuesto adicional solicitado.</b>')

      return
    }

    if (valoresCorrectos) {

      this.guardar.emit(true);

    } else {
      this.openDialog('', '<b>La sumatoria de los componentes, no es igual el valor total del aporte.</b>');
    }

  }

  getAportanteById(id: number, i: number) {
    let cofinanciacionAportante = this.novedad.novedadContractualAportante[i].cofinanciacionAportante;
    this.commonService.listaAportanteByTipoAportanteId(id).subscribe(
      respuesta => {
        let respuestaok = respuesta.filter(x => x.registroCompleto == true && x.tieneFuentes == true && !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId));
        this.listaTotalNuevas = respuesta.filter(x => x.registroCompleto == true && !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId)  && !this.aportantes.controls.some(aportante => aportante.get('cofinanciacionAportanteId').value === x.cofinanciacionAportanteId));

        this.listaVigencias[i] = [];
        this.listaAportante[i] = [];
        if (this.tipoAportante.FFIE.includes(id.toString())) {
          this.listaVigencias[i] = respuestaok.filter(x => !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId));
        } else {
          if (this.tipoAportante.ET.includes(id.toString())) {
            this.listaAportante[i] = respuestaok.filter(x => !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId || e.nombreAportanteString.includes(x.nombre)));

            this.commonService.listaDepartamentos().subscribe(res => {
              this.listadoDepto[i] = res.sort((a, b) => {
                let textA = a.descripcion.toUpperCase();
                let textB = b.descripcion.toUpperCase();
                return textA < textB ? -1 : textA > textB ? 1 : 0;
              });
              this.commonService
                .listaMunicipiosByIdDepartamento(cofinanciacionAportante.departamentoId)
                .subscribe(res => {
                  this.listadoMun[i] = res.sort((a, b) => {
                    let textA = a.descripcion.toUpperCase();
                    let textB = b.descripcion.toUpperCase();
                    return textA < textB ? -1 : textA > textB ? 1 : 0;
                  });
                  this.getVigenciaByMun(cofinanciacionAportante.municipioId, i);
                });
            });
          } else {
            this.listaAportante[i] = respuestaok.filter(x => !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId || e.nombreAportanteString == x.nombre));
            this.listaNombreAportantes[i] = [];
            respuestaok.filter(x => !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId || e.nombreAportanteString == x.nombre)).forEach(element => {
              if (!this.listaNombreAportantes[i].includes(element.nombre)) {
                this.listaNombreAportantes[i].push(element.nombre);                
              }
            });
            this.getVigencia(this.novedad.novedadContractualAportante[i].nombreAportante, i);            
          }
        }
      },
      err => {
        let mensaje: string;
        if (err.message) {
          mensaje = err.message;
        } else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
      }
    );
  }
  getAportante(event: any, i: number) {
    this.commonService.listaAportanteByTipoAportanteId(event).subscribe(
      respuesta => {
        let respuestaok = respuesta.filter(x => x.registroCompleto == true && x.tieneFuentes == true && !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId) && !this.aportantes.controls.some(aportante => aportante.get('cofinanciacionAportanteId').value === x.cofinanciacionAportanteId));
        this.listaTotalNuevas = respuesta.filter(x => x.registroCompleto == true && x.tieneFuentes == true && !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId)  && !this.aportantes.controls.some(aportante => aportante.get('cofinanciacionAportanteId').value === x.cofinanciacionAportanteId));
        
        this.listaVigencias[i] = [];
        this.listaAportante[i] = [];
        this.listadoDepto[i] = [];
        this.listaNombreAportantes[i] = [];

        this.aportantes.controls[i].get('cofinanciacionAportanteId').setValue(null);
        this.aportantes.controls[i].get('depaetamento').setValue(null);
        this.aportantes.controls[i].get('municipio').setValue(null);
        this.aportantes.controls[i].get('nombreAportante').setValue(null);

        if (this.tipoAportante.FFIE.includes(event.toString())) {
          this.listaVigencias[i] = respuestaok.filter(x => !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId)  && !this.aportantes.controls.some(aportante => aportante.get('cofinanciacionAportanteId').value === x.cofinanciacionAportanteId));
        } else {
          if (this.tipoAportante.ET.includes(event.toString())) {
            this.listaAportante[i] = respuestaok.filter(x => !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId || e.nombreAportanteString.includes(x.nombre))  && !this.aportantes.controls.some(aportante => aportante.get('cofinanciacionAportanteId').value === x.cofinanciacionAportanteId));
            this.listadoDepto[i] = [{ localizacionId: null, descripcion: 'un momento por favor.' }];
            this.commonService.listaDepartamentos().subscribe(res => {
              this.listadoDepto[i] = res.sort((a, b) => {
                let textA = a.descripcion.toUpperCase();
                let textB = b.descripcion.toUpperCase();
                return textA < textB ? -1 : textA > textB ? 1 : 0;
              });
            });
            this.listadoMun[i] = [];
          } else {
            this.listaAportante[i] = respuestaok.filter(x => !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId || e.nombreAportanteString == x.nombre)  && !this.aportantes.controls.some(aportante => aportante.get('cofinanciacionAportanteId').value === x.cofinanciacionAportanteId));
            this.listaNombreAportantes[i] = [];
            respuestaok.filter(x => !this.listaAportantes.some(e => e.cofinanciacionAportanteId === x.cofinanciacionAportanteId || e.nombreAportanteString == x.nombre)  && !this.aportantes.controls.some(aportante => aportante.get('cofinanciacionAportanteId').value === x.cofinanciacionAportanteId)).forEach(element => {
              if (!this.listaNombreAportantes[i].includes(element.nombre)) {
                this.listaNombreAportantes[i].push(element.nombre);
              }
            });
          }
        }
      },
      err => {
        let mensaje: string;
        if (err.message) {
          mensaje = err.message;
        } else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
      }
    );
  }

  changeDepartamento(e) {
    const idDepartamento = e.localizacionId;
    this.commonService.listaMunicipiosByIdDepartamento(idDepartamento).subscribe(mun => {
      this.listaMunicipios = mun.sort((a, b) => {
        let textA = a.descripcion.toUpperCase();
        let textB = b.descripcion.toUpperCase();
        return textA < textB ? -1 : textA > textB ? 1 : 0;
      });
    });
  }

  getMunAportante(id: any, i: number) {
    this.listaVigencias[i] = this.listaAportante[i].filter(
      x => x.departamentoId == id && x.municipioId == null
    );
    this.commonService.listaMunicipiosByIdDepartamento(id).subscribe(res => {
      this.listadoMun[i] = res.sort((a, b) => {
        let textA = a.descripcion.toUpperCase();
        let textB = b.descripcion.toUpperCase();
        return textA < textB ? -1 : textA > textB ? 1 : 0;
      });
    });
  }

  getVigenciaByMun(id: any, i: number) {
    this.listaVigencias[i] = this.listaAportante[i].filter(
      x => x.municipioId == id
    );
  }

  getVigencia(nombre: any, i: number) {
    this.listaVigencias[i] = this.listaAportante[i].filter(
      x => x.nombre == nombre
    );
  }

  updateOldAportante() {
    for( var i=this.listaAportantes.length - 1; i>=0; i--){
       for( var j=0; j<this.aportantes.controls.length; j++){
           if(this.listaAportantes[i] && (this.listaAportantes[i].cofinanciacionAportanteId === this.aportantes.controls[j].get('cofinanciacionAportanteId').value)){
            this.listaAportantes.splice(i, 1);
          }
        }
    }
  }
}
