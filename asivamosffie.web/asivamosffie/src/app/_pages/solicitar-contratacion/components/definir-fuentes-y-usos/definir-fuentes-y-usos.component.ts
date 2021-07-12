import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormGroup, FormControl } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { ContratacionProyecto, ComponenteUso, ComponenteAportante, ContratacionProyectoAportante } from 'src/app/_interfaces/project-contracting';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { forkJoin } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-definir-fuentes-y-usos',
  templateUrl: './definir-fuentes-y-usos.component.html',
  styleUrls: ['./definir-fuentes-y-usos.component.scss']
})

export class DefinirFuentesYUsosComponent implements OnInit, OnDestroy {

  idSolicitud: number;
  contratacionProyecto: ContratacionProyecto;
  municipio: string;
  tipoIntervencion: string;
  addressForm = this.fb.group([]);
  fasesSelect: Dominio[] = [];
  listaUsos: any[] = [];
  listaComponentes: any[] = [];
  componentesSelect: Dominio[] = [];
  usosSelect: Dominio[] = [];
  realizoPeticion: boolean = false;
  esSaldoPermitido: boolean = false;
  listaFaseUsosComponentes: any[] = [];
  estaEditando = false;

  createFormulario() {
    return this.fb.group({
      aportantes: this.fb.array([])
    });
  }

  componentes(i: number) {
    return this.aportantes.controls[i].get('componentes') as FormArray;
  }

  get aportantes() {
    return this.addressForm.get('aportantes') as FormArray;
  }

  usos(j: number, i: number) {
    // const control = this.addressForm.get('componentes') as FormArray;
    // return control.controls[i].get('usos') as FormArray;
    return this.componentes(j).controls[i].get('usos') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private projectContractingService: ProjectContractingService,
    private commonService: CommonService,
    public dialog: MatDialog,
    private router: Router )
  {
    this.getMunicipio();
  }

  ngOnDestroy(): void {
    if (this.addressForm.dirty && this.realizoPeticion === false && this.esSaldoPermitido === true) {
      this.openDialogConfirmar('', '¿Desea guardar la información registrada?');
    }
  };

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
      nombreAportante: [],
      fasesSelect: [ null ],
      estadoSemaforo: [null],
      saldoDisponible: [null],
      contratacionProyectoAportanteId: [],
      proyectoAportanteId: [],
      listaFuenteFinanciacion: [ null ],
      valorAportanteProyecto: [ { value: null, disabled: true }, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)]),
      ],
      componentes: this.fb.array([])
    });
  };

  ngOnInit(): void {

    this.addressForm = this.createFormulario();

    this.route.params.subscribe((params: Params) => {
      const id = params.id;

      forkJoin([
        this.commonService.listaFases(),
        this.commonService.listaComponentes(),
        this.commonService.listaUsos(),
        this.projectContractingService.getContratacionProyectoById(id),
        this.projectContractingService.getListFaseComponenteUso(),
      ])

        .subscribe( async response => {

          this.fasesSelect = response[0];
          this.componentesSelect = response[1];
          this.usosSelect = response[2];
          this.contratacionProyecto = response[3];
          this.listaFaseUsosComponentes = response[4];
          const listaFuenteTipoFinanciacion = await this.commonService.listaFuenteTipoFinanciacion().toPromise();
          console.log( this.contratacionProyecto )

          setTimeout(() => {
            const interventoria = '2';
            let esInterventoria = true;

            if ( this.contratacionProyecto['contratacion'].tipoSolicitudCodigo !== interventoria ) {
              esInterventoria = false;
            }

            if (this.componentesSelect.length > 0) {
              this.listaComponentes = this.componentesSelect.filter(value => this.contratacionProyecto['contratacion'].tipoSolicitudCodigo === value.codigo);
            };

            this.idSolicitud = this.contratacionProyecto.contratacionId;

            this.commonService.listaTipoIntervencion()
              .subscribe((intervenciones: any) => {
                for (const intervencion of intervenciones) {
                  if (intervencion.codigo === this.contratacionProyecto.proyecto.tipoIntervencionCodigo) {
                    this.tipoIntervencion = intervencion.nombre;
                    break;
                  }
                }
              });

            this.contratacionProyecto.contratacionProyectoAportante.forEach( async apo => {
              const listaFase = [ ...this.fasesSelect ];
              const grupoAportante = this.createAportante();
              const listaComponentes = grupoAportante.get('componentes') as FormArray;
              const valorAportanteProyecto = esInterventoria === true ? apo[ 'cofinanciacionAportante' ].proyectoAportante[ 0 ].valorInterventoria : apo[ 'cofinanciacionAportante' ].proyectoAportante[ 0 ].valorObra;
              let listaFuenteFinanciacion = [];
              apo[ 'cofinanciacionAportante' ].fuenteFinanciacion.forEach( fuente => {
                const fuenteFind = listaFuenteTipoFinanciacion.find( fuenteTipo => fuenteTipo.codigo === fuente.fuenteRecursosCodigo );

                if ( fuenteFind !== undefined ) {
                  fuente.nombreFuente = fuenteFind.nombre;
                }
                if(fuente?.eliminado !== true && fuente?.eliminado !== 'undefined'){
                  listaFuenteFinanciacion.push(fuente);
                }
              } )
              grupoAportante.get( 'listaFuenteFinanciacion' ).setValue(listaFuenteFinanciacion);
              grupoAportante.get('contratacionProyectoAportanteId').setValue(apo.contratacionProyectoAportanteId);
              grupoAportante.get('proyectoAportanteId').setValue(apo.proyectoAportanteId);
              if (apo.valorAporte !== 0) {
                this.esSaldoPermitido = true;
              }
              grupoAportante.get('valorAportanteProyecto').setValue( valorAportanteProyecto );
              grupoAportante.get('saldoDisponible').setValue(apo['saldoDisponible'] ? apo['saldoDisponible'] : 0);
              if (apo['cofinanciacionAportante'].tipoAportanteId === 6) {
                grupoAportante.get('nombreAportante').setValue('FFIE');
              } else if (apo['cofinanciacionAportante'].tipoAportanteId === 9) {
                if (apo['cofinanciacionAportante'].departamento !== undefined && apo['cofinanciacionAportante'].municipio === undefined) {
                  grupoAportante.get('nombreAportante').setValue(`Gobernación de ${apo['cofinanciacionAportante'].departamento.descripcion}`);
                };
                if (apo['cofinanciacionAportante'].departamento !== undefined && apo['cofinanciacionAportante'].municipio !== undefined) {
                  grupoAportante.get('nombreAportante').setValue(`Alcaldía de ${apo['cofinanciacionAportante'].municipio.descripcion}`);
                };
              } else if (apo['cofinanciacionAportante'].tipoAportanteId === 10) {
                grupoAportante.get('nombreAportante').setValue(`${apo['cofinanciacionAportante'].nombreAportante.nombre}`);
              }

              if (apo.componenteAportante.length > 0) {
                apo.componenteAportante.forEach(compoApo => {
                  const grupoComponente = this.createComponente();
                  const listaUsos = grupoComponente.get('usos') as FormArray;
                  const faseSeleccionada = this.fasesSelect.find(f => f.codigo == compoApo.faseCodigo);
                  const componenteSeleccionado = this.componentesSelect.find(c => c.codigo == compoApo.tipoComponenteCodigo);

                  if ( faseSeleccionada !== undefined ) {
                    const indexFase = listaFase.findIndex( fase => fase === faseSeleccionada );

                    if ( indexFase !== -1 ) {
                      listaFase.splice( indexFase, 1 );
                    }
                  }

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

                  grupoComponente.get('componenteAportanteId').setValue(compoApo.componenteAportanteId);
                  grupoComponente.get('contratacionProyectoAportanteId').setValue(compoApo.contratacionProyectoAportanteId);
                  grupoComponente.get('fase').setValue( faseSeleccionada !== undefined ? faseSeleccionada : null );
                  grupoComponente.get('componente').setValue( componenteSeleccionado !== undefined ? componenteSeleccionado : null );
                  grupoComponente.get('listaUsos').setValue(listaDeUsos)

                  compoApo.componenteUso.forEach(uso => {
                    const grupoUso = this.createUso();
                    const usoSeleccionado = this.usosSelect.find(u => u.codigo == uso.tipoUsoCodigo);

                    grupoUso.get( 'fuenteFinanciacionId' ).setValue( uso[ 'fuenteFinanciacionId' ] !== undefined ? uso[ 'fuenteFinanciacionId' ] : null )
                    grupoUso.get('componenteUsoId').setValue(uso.componenteUsoId);
                    grupoUso.get('componenteAportanteId').setValue(uso.componenteAportanteId);
                    grupoUso.get('usoDescripcion').setValue(usoSeleccionado);
                    grupoUso.get('valorUso').setValue(uso.valorUso);

                    if (grupoAportante.get('valorAportanteProyecto').value === 0 && grupoUso.get('valorUso').value === 0) {
                      grupoAportante.get('estadoSemaforo').setValue('sin-diligenciar');
                    }

                    listaUsos.push(grupoUso);
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
              grupoAportante.get( 'fasesSelect' ).setValue( listaFase );
              this.aportantes.push(grupoAportante);
            });

          }, 1300);
        });


    });

  }

  changeFase(posicionAportante, posicionComponente) {

    const listFase: Dominio[] = this.aportantes.controls[ posicionAportante ].get( 'fasesSelect' ).value;
    const faseSeleccionada = this.componentes( posicionAportante ).controls[ posicionComponente ].get( 'fase' ).value;
    const faseIndex = listFase.findIndex( fase => fase === faseSeleccionada );

    if ( faseIndex !== -1 ) {
      listFase.splice( faseIndex, 1 );
      this.aportantes.controls[ posicionAportante ].get( 'fasesSelect' ).setValue( listFase );
    }

    this.componentes(posicionAportante).controls[posicionComponente].get('componente').setValue(null);
    this.usos(posicionAportante, posicionComponente).controls.forEach(control => {
      control.get('usoDescripcion').setValue(null);
    })

  }

  getListaUsosFiltrado(posicionAportante, posicionComponente, posicionUso) {

    let usoSeleccionado = this.usos(posicionAportante, posicionComponente).controls[posicionUso];

    //let listaUsos =this.componentes(posicionAportante).controls[posicionComponente].get('listaUsos').value.map((x) => x);
    let listaUsos: any[] = [];

    if ( this.componentes(posicionAportante).controls[posicionComponente].get('listaUsos').value )
    this.componentes(posicionAportante).controls[posicionComponente].get('listaUsos').value.forEach(u => {
        listaUsos.push(u)
    });

    this.usos(posicionAportante, posicionComponente).controls.forEach(u => {

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
        this.componentes(posicionAportante).controls[posicionComponente].get('listaUsos').setValue(listaDeUsos)
        //this.listaUsos = listaDeUsos;
      };
    };
  };

  validarSaldoDisponible(saldoIngresado: number, saldoDisponible: number, nombreAportante: string) {
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

  getMunicipio() {
    if (this.router.getCurrentNavigation().extras.replaceUrl || this.router.getCurrentNavigation().extras.skipLocationChange === false) {
      this.router.navigate(['/solicitarContratacion']);
      return;
    };
    this.municipio = this.router.getCurrentNavigation().extras.state.municipio;
  };

  addUso(j: number, i: number) {
    if ( this.componentes(j).controls[i].get('listaUsos').value !== null ) {
      this.componentes(j).controls[i].get('listaUsos').value.length
      if (this.componentes(j).controls[i].get('listaUsos').value.length === this.usos(j, i).controls.length) {
        this.openDialog('', `<b>No se encuentran usos disponibles para el componente de ${this.contratacionProyecto['contratacion'].tipoSolicitudCodigo === '2' ? 'Interventoría' : 'Obra'}.</b>`);
        return;
      };
      const listaUsos = this.componentes(j).controls[i].get('usos') as FormArray;
      listaUsos.push(this.createUso());
    }
  }

  deleteUso(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  createUso(): FormGroup {
    return this.fb.group({
      componenteUsoId: [],
      componenteAportanteId: [],
      fuenteFinanciacionId: [ null, Validators.required ],
      usoDescripcion: [null, Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(20)])],
      valorUso: [null, Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(20)])
      ],
      listaUsos: []
    });
  }

  addComponent(i: number, j: number) {
    if ( this.aportantes.controls[ i ].get( 'fasesSelect' ).value.length > 0 ) {
      let grupoComponente = this.createComponente();
      let listaUsos = grupoComponente.get('usos') as FormArray;
      listaUsos.push(this.createUso());
      this.componentes(i).push(grupoComponente);
    } else {
      this.openDialog( '', '<b>Ha utilizado todas las fases disponibles. No pueden agregar más componentes.</b>' );
    }
  }

  createComponente(): FormGroup {
    return this.fb.group({
      componenteAportanteId: [],
      contratacionProyectoAportanteId: [],
      fase: [null, Validators.required],
      componente: [ null, Validators.required ],
      usos: this.fb.array([]),
      listaUsos: this.listaUsos,
    });
  }

  borrarArray(j: number, i: number) {
    this.openDialogTrueFalse('', '<b>¿Está seguro de eliminar esta información?</b>')
      .subscribe(
        value => {
          if (value === true) {
            const listFase: Dominio[] = this.aportantes.controls[ j ].get( 'fasesSelect' ).value;
            const faseSeleccionada = this.componentes( j ).controls[ i ].get( 'fase' ).value;

            if ( faseSeleccionada !== null ) {
              listFase.push( faseSeleccionada );
            }
            this.aportantes.controls[ j ].get( 'fasesSelect' ).setValue( listFase );

            if (this.componentes(j).at(i).get('componenteAportanteId').value !== null) {
              this.projectContractingService.deleteComponenteAportante(this.componentes(j).at(i).get('componenteAportanteId').value)
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

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    let valoresCorrectos = true;
    let valorTotalSumado = 0;
    let totalAportantes = 0;

    this.contratacionProyecto.contratacionProyectoAportante = [];

    this.aportantes.controls.forEach(controlAportante => {
      const listaComponentes = controlAportante.get('componentes') as FormArray;

      if (controlAportante.get('valorAportanteProyecto').value !== 0) {
        totalAportantes++;
      };

      valorTotalSumado += controlAportante.get('valorAportanteProyecto').value;

      const aportante: ContratacionProyectoAportante = {
        contratacionProyectoAportanteId: controlAportante.get('contratacionProyectoAportanteId').value,
        contratacionProyectoId: this.contratacionProyecto.contratacionProyectoId,
        proyectoAportanteId: controlAportante.get('proyectoAportanteId').value,
        valorAporte: controlAportante.get('valorAportanteProyecto').value,
        componenteAportante: []
      };

      let valorTotal = 0;
      let valorSumado = 0;

      valorTotal = aportante.valorAporte;

      listaComponentes.controls.forEach(controlComponente => {
        const listaUsos = controlComponente.get('usos') as FormArray;

        const componenteAportante: ComponenteAportante = {
          componenteAportanteId: controlComponente.get('componenteAportanteId').value,
          contratacionProyectoAportanteId: aportante.contratacionProyectoAportanteId,
          tipoComponenteCodigo: controlComponente.get('componente').value ? controlComponente.get('componente').value.codigo : null,
          faseCodigo: controlComponente.get('fase').value ? controlComponente.get('fase').value.codigo : null,
          componenteUso: []
        };

        listaUsos.controls.forEach(controlUsos => {
          const componenteUso: any = {
            componenteUsoId: controlUsos.get('componenteUsoId').value,
            componenteAportanteId: componenteAportante.componenteAportanteId,
            fuenteFinanciacionId: controlUsos.get( 'fuenteFinanciacionId' ).value,
            tipoUsoCodigo: controlUsos.get('usoDescripcion').value ? controlUsos.get('usoDescripcion').value.codigo : null,
            valorUso: controlUsos.get('valorUso').value,
          };

          valorSumado = valorSumado + componenteUso.valorUso;

          componenteAportante.componenteUso.push(componenteUso);
        });

        aportante.componenteAportante.push(componenteAportante);

      });

      if (valorSumado != valorTotal) {

        valoresCorrectos = false;
      };

      this.contratacionProyecto.contratacionProyectoAportante.push(aportante);
    });

    if (this.contratacionProyecto['contratacion'].tipoSolicitudCodigo === '1' && this.aportantes.controls[0].get('valorAportanteProyecto').value === this.contratacionProyecto.proyecto.valorObra && totalAportantes !== this.aportantes.controls.length) {
      this.openDialog('', '<b>Debe distribuir el valor total del proyecto entre todo los aportantes.</b>');
      return;
    };
    if (this.contratacionProyecto['contratacion'].tipoSolicitudCodigo === '2' && this.aportantes.controls[0].get('valorAportanteProyecto').value === this.contratacionProyecto.proyecto.valorInterventoria && totalAportantes !== this.aportantes.controls.length) {
      this.openDialog('', '<b>Debe distribuir el valor total del proyecto entre todo los aportantes.</b>');
      return;
    };
    if (this.contratacionProyecto['contratacion'].tipoSolicitudCodigo === '2' && totalAportantes === this.aportantes.controls.length) {
      if (valorTotalSumado !== this.contratacionProyecto.proyecto.valorInterventoria) {
        this.openDialog('', '<b>El valor del aporte no corresponde con el valor requerido en la solicitud de interventoría.</b>');
        return;
      };
    };

    if (this.contratacionProyecto['contratacion'].tipoSolicitudCodigo === '1' && totalAportantes === this.aportantes.controls.length) {
      if (valorTotalSumado !== this.contratacionProyecto.proyecto.valorObra) {
        this.openDialog('', '<b>El valor del aporte no corresponde con el valor requerido en la solicitud de obra.</b>');
        return;
      };
    };

    // if (valoresCorrectos) {

      this.projectContractingService.createEditContratacionProyectoAportanteByContratacionproyecto(this.contratacionProyecto)
        .subscribe(
          respuesta => {
            this.openDialog('', `<b>${respuesta.message}</b>`);
            this.realizoPeticion = true;
            if (respuesta.code === '200') {
              //this.router.navigate(['/solicitarContratacion/solicitud', this.contratacionProyecto.contratacionId]);
              this.ngOnInit();

            }
          },
          err => this.openDialog('', `<b>${err.message}</b>`)
        );

    // } else {
    //   this.openDialog('', '<b>La sumatoria de los componentes, no es igual el valor total del aporte.</b>');
    // }

  }
}
