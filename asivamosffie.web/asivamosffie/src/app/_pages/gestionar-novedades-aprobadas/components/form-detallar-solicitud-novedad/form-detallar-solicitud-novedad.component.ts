import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ComponenteAportanteNovedad, ComponenteUsoNovedad, NovedadContractual, NovedadContractualAportante } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-form-detallar-solicitud-novedad',
  templateUrl: './form-detallar-solicitud-novedad.component.html',
  styleUrls: ['./form-detallar-solicitud-novedad.component.scss']
})
export class FormDetallarSolicitudNovedadComponent implements OnInit {

  @Input() novedad: NovedadContractual;
  @Output() guardar = new EventEmitter();

  //idSolicitud: number;
  //municipio: string;
  //tipoIntervencion: string;
  addressForm = this.fb.group([]);
  fasesSelect: Dominio[] = [];
  listaUsos: any[] = [];
  listaAportantes: any[] = [];
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
    private contractualNoveltyService: ContractualNoveltyService,
    private projectContractingService: ProjectContractingService,
    private commonService: CommonService,
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
      nombreAportante: [],
      estadoSemaforo: [null],
      saldoDisponible: [null],
      contratacionProyectoAportanteId: [],
      cofinanciacionAportanteId:[],
      proyectoAportanteId: [],
      valorAportanteProyecto: [null, Validators.compose([
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
        this.projectContractingService.getListFaseComponenteUso(),
        this.contractualNoveltyService.GetAportanteByContratacion( this.novedad.contrato.contratacionId )
        //this.projectContractingService.getContratacionProyectoById(id),
      ])

        .subscribe(response => {

          this.fasesSelect = response[0];
          this.componentesSelect = response[1];
          this.usosSelect = response[2];
          this.listaFaseUsosComponentes = response[3];
          this.listaAportantes = response[4];
          //this.contratacionProyecto = response[5];

          setTimeout(() => {

            if (this.componentesSelect.length > 0) {
              this.listaComponentes = this.componentesSelect.filter(value => this.novedad.contrato.contratacion.tipoSolicitudCodigo === value.codigo);
            };

            //this.idSolicitud = this.contratacionProyecto.contratacionId;

            // this.commonService.listaTipoIntervencion()
            //   .subscribe((intervenciones: any) => {
            //     for (const intervencion of intervenciones) {
            //       if (intervencion.codigo === this.contratacionProyecto.proyecto.tipoIntervencionCodigo) {
            //         this.tipoIntervencion = intervencion.nombre;
            //         break;
            //       }
            //     }
            //   });

            if (this.novedad.novedadContractualAportante === undefined || this.novedad.novedadContractualAportante.length === 0){
              const grupoAportante = this.createAportante();
              const listaComponentes = grupoAportante.get('componentes') as FormArray;

              const grupoComponente = this.createComponente();
              const listaUsos = grupoComponente.get('usos') as FormArray;

              const grupoUso = this.createUso();
              listaUsos.push(grupoUso);

              listaComponentes.push(grupoComponente);
              this.aportantes.push(grupoAportante);

              console.log('aa')
            }

            this.novedad.novedadContractualAportante.forEach(apo => {
              const grupoAportante = this.createAportante();
              const listaComponentes = grupoAportante.get('componentes') as FormArray;

              grupoAportante.get('contratacionProyectoAportanteId').setValue(apo.novedadContractualAportanteId);
              grupoAportante.get('proyectoAportanteId').setValue(apo.cofinanciacionAportanteId);
              if (apo.valorAporte !== 0) {
                this.esSaldoPermitido = true;
              }
              grupoAportante.get('valorAportanteProyecto').setValue(apo.valorAporte);
              grupoAportante.get('saldoDisponible').setValue(apo['saldoDisponible'] ? apo['saldoDisponible'] : 0);
              grupoAportante.get('cofinanciacionAportanteId').setValue(apo.cofinanciacionAportanteId);
              //if (apo['cofinanciacionAportante'].tipoAportanteId === 6) {
              //  grupoAportante.get('nombreAportante').setValue('FFIE');
              //} else if (apo['cofinanciacionAportante'].tipoAportanteId === 9) {
              //  if (apo['cofinanciacionAportante'].departamento !== undefined && apo['cofinanciacionAportante'].municipio === undefined) {
              //    grupoAportante.get('nombreAportante').setValue(`Gobernación de ${apo['cofinanciacionAportante'].departamento.descripcion}`);
              //  };
              //  if (apo['cofinanciacionAportante'].departamento !== undefined && apo['cofinanciacionAportante'].municipio !== undefined) {
              //    grupoAportante.get('nombreAportante').setValue(`Alcaldía de ${apo['cofinanciacionAportante'].municipio.descripcion}`);
              //  };
              //} else if (apo['cofinanciacionAportante'].tipoAportanteId === 10) {
              //  grupoAportante.get('nombreAportante').setValue(`${apo['cofinanciacionAportante'].nombreAportante.nombre}`);
              //}

              if (apo.componenteAportanteNovedad.length > 0) {
                apo.componenteAportanteNovedad.forEach(compoApo => {
                  const grupoComponente = this.createComponente();
                  const listaUsos = grupoComponente.get('usos') as FormArray;
                  const faseSeleccionada = this.fasesSelect.find(f => f.codigo == compoApo.faseCodigo);
                  const componenteSeleccionado = this.componentesSelect.find(c => c.codigo == compoApo.tipoComponenteCodigo);

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

                  
                  grupoComponente.get('componenteAportanteId').setValue(compoApo.componenteAportanteNovedadId);
                  grupoComponente.get('contratacionProyectoAportanteId').setValue(compoApo.novedadContractualAportanteId);
                  grupoComponente.get('fase').setValue(faseSeleccionada);
                  grupoComponente.get('componente').setValue(componenteSeleccionado);
                  grupoComponente.get('listaUsos').setValue(listaDeUsos)

                  compoApo.componenteUsoNovedad.forEach(uso => {
                    const grupoUso = this.createUso();
                    const usoSeleccionado = this.usosSelect.find(u => u.codigo == uso.tipoUsoCodigo);

                    grupoUso.get('componenteUsoId').setValue(uso.componenteUsoNovedadId);
                    grupoUso.get('componenteAportanteId').setValue(uso.componenteAportanteNovedadId);
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

              this.aportantes.push(grupoAportante);
            });

          }, 1000);
        });


    });

  }

  changeFase(posicionAportante, posicionComponente) {

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
    // if (saldoIngresado > saldoDisponible) {
    //   this.openDialog('', `<b>El valor del aportante ${nombreAportante} al proyecto es superior al valor disponible, verifique por favor con él área financiera.</b>`);
    //   this.esSaldoPermitido = false;
    // } else if (saldoIngresado <= saldoDisponible) {
    this.esSaldoPermitido = true;
    // };
  };

  deleteUsoSeleccionado(usoCodigo: any) {
    this.listaUsos = this.listaUsos.filter(uso => uso.codigo !== usoCodigo);
  };


  addUso(j: number, i: number) {
    this.componentes(j).controls[i].get('listaUsos').value.length
     if (this.componentes(j).controls[i].get('listaUsos').value.length === this.usos(j, i).controls.length) {
       this.openDialog('', `<b>No se encuentran usos disponibles para el componente de ${this.novedad.contrato.contratacion.tipoSolicitudCodigo === '2' ? 'Interventoria' : 'Obra'}.</b>`);
       return;
     };
    const listaUsos = this.componentes(j).controls[i].get('usos') as FormArray;
    listaUsos.push(this.createUso());
  }

  deleteUso(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
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

  addComponent(i: number) {
    let grupoComponente = this.createComponente();
    let listaUsos = grupoComponente.get('usos') as FormArray;
    listaUsos.push(this.createUso());
    this.componentes(i).push(grupoComponente);
  }

  createComponente(): FormGroup {
    return this.fb.group({
      componenteAportanteId: [],
      contratacionProyectoAportanteId: [],
      fase: [null, Validators.required],
      componente: [null, Validators.required],
      usos: this.fb.array([]),
      listaUsos: this.listaUsos,
    });
  }

  borrarArray(j: number, i: number) {
    this.openDialogTrueFalse('', '<b>¿Está seguro de eliminar esta información?</b>')
      .subscribe(
        value => {
          if (value === true) {
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

    this.novedad.novedadContractualAportante = [];

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

      listaComponentes.controls.forEach(controlComponente => {
        const listaUsos = controlComponente.get('usos') as FormArray;

        const componenteAportante: ComponenteAportanteNovedad = {
          componenteAportanteNovedadId: controlComponente.get('componenteAportanteId').value,
          cofinanciacionAportanteId: aportante.cofinanciacionAportanteId,
          tipoComponenteCodigo: controlComponente.get('componente').value ? controlComponente.get('componente').value.codigo : null,
          faseCodigo: controlComponente.get('fase').value ? controlComponente.get('fase').value.codigo : null,
          componenteUsoNovedad: []
        };

        listaUsos.controls.forEach(controlUsos => {
          const componenteUso: ComponenteUsoNovedad = {
            componenteUsoNovedadId: controlUsos.get('componenteUsoId').value,
            componenteAportanteNovedadId: componenteAportante.componenteAportanteNovedadId,
            tipoUsoCodigo: controlUsos.get('usoDescripcion').value ? controlUsos.get('usoDescripcion').value.codigo : null,
            valorUso: controlUsos.get('valorUso').value,
          };

          valorSumado = valorSumado + componenteUso.valorUso;

          componenteAportante.componenteUsoNovedad.push(componenteUso);
        });

        aportante.componenteAportanteNovedad.push(componenteAportante);

      });

      if (valorSumado != valorTotal) {

        valoresCorrectos = false;
      };

      this.novedad.novedadContractualAportante.push(aportante);
    });

    // if (this.contratacionProyecto['contratacion'].tipoSolicitudCodigo === '1' && this.aportantes.controls[0].get('valorAportanteProyecto').value === this.contratacionProyecto.proyecto.valorObra && totalAportantes !== this.aportantes.controls.length) {
    //   this.openDialog('', '<b>Debe distribuir el valor total del proyecto entre todo los aportantes.</b>');
    //   return;
    // };
    // if (this.contratacionProyecto['contratacion'].tipoSolicitudCodigo === '2' && this.aportantes.controls[0].get('valorAportanteProyecto').value === this.contratacionProyecto.proyecto.valorInterventoria && totalAportantes !== this.aportantes.controls.length) {
    //   this.openDialog('', '<b>Debe distribuir el valor total del proyecto entre todo los aportantes.</b>');
    //   return;
    // };
    // if (this.contratacionProyecto['contratacion'].tipoSolicitudCodigo === '2' && totalAportantes === this.aportantes.controls.length) {
    //   if (valorTotalSumado !== this.contratacionProyecto.proyecto.valorInterventoria) {
    //     this.openDialog('', '<b>El valor del aporte no corresponde con el valor requerido en la solicitud de interventoría.</b>');
    //     return;
    //   };
    // };

    // if (this.contratacionProyecto['contratacion'].tipoSolicitudCodigo === '1' && totalAportantes === this.aportantes.controls.length) {
    //   if (valorTotalSumado !== this.contratacionProyecto.proyecto.valorObra) {
    //     this.openDialog('', '<b>El valor del aporte no corresponde con el valor requerido en la solicitud de obra.</b>');
    //     return;
    //   };
    // };

    if (valoresCorrectos) {

      this.guardar.emit(true);

    } else {
      this.openDialog('', '<b>La sumatoria de los componentes, no es igual el valor total del aporte.</b>');
    }

  }
}
