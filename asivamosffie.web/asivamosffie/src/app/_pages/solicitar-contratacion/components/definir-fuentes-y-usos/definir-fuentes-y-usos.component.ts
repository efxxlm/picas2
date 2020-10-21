import { Component, OnInit } from '@angular/core';
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

export class DefinirFuentesYUsosComponent implements OnInit {

  idSolicitud: number;
  contratacionProyecto: ContratacionProyecto;
  municipio: string;
  tipoIntervencion: string;

  addressForm = this.fb.group([]);

  fasesSelect: Dominio[] = [];

  componentesSelect: Dominio[] = [];

  usosSelect: Dominio[] = [];


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
    private router: Router,

  ) {
    this.getMunicipio();
  }

  createAportante() {
    return this.fb.group({
      nombreAportante: [],
      estadoSemaforo: [ null ],
      contratacionProyectoAportanteId: [],
      proyectoAportanteId: [],
      valorAportanteProyecto: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)]),
      ],
      componentes: this.fb.array([])
    });
  }

  ngOnInit(): void {

    this.addressForm = this.createFormulario();

    this.route.params.subscribe((params: Params) => {
      const id = params.id;

      forkJoin([
        this.commonService.listaFases(),
        this.commonService.listaComponentes(),
        this.commonService.listaUsos(),
        this.projectContractingService.getContratacionProyectoById(id)
      ])

        .subscribe(response => {

          this.fasesSelect = response[0];
          this.componentesSelect = response[1];
          this.usosSelect = response[2];
          this.contratacionProyecto = response[3];

          setTimeout(() => {

            this.commonService.listaTipoIntervencion()
              .subscribe((intervenciones: any) => {
                for (const intervencion of intervenciones) {
                  if (intervencion.codigo === this.contratacionProyecto.proyecto.tipoIntervencionCodigo) {
                    this.tipoIntervencion = intervencion.nombre;
                    break;
                  }
                }
              });

            this.contratacionProyecto.contratacionProyectoAportante.forEach(apo => {
              const grupoAportante = this.createAportante();
              const listaComponentes = grupoAportante.get('componentes') as FormArray;

              grupoAportante.get('contratacionProyectoAportanteId').setValue(apo.contratacionProyectoAportanteId);
              grupoAportante.get('proyectoAportanteId').setValue(apo.proyectoAportanteId);
              grupoAportante.get('valorAportanteProyecto').setValue(apo.valorAporte);

              if (apo['cofinanciacionAportante'].tipoAportanteId === 6) {
                grupoAportante.get('nombreAportante').setValue('FFIE');
              } else if (apo['cofinanciacionAportante'].tipoAportanteId === 9) {
                grupoAportante.get('nombreAportante').setValue(`${apo['cofinanciacionAportante'].departamento.descripcion} / ${apo['cofinanciacionAportante'].municipio.descripcion}`);
              } else if (apo['cofinanciacionAportante'].tipoAportanteId === 10) {
                grupoAportante.get('nombreAportante').setValue(`${apo['cofinanciacionAportante'].nombreAportante.nombre}`);
              }

              if (apo.componenteAportante.length > 0) {
                apo.componenteAportante.forEach(compoApo => {
                  const grupoComponente = this.createComponente();
                  const listaUsos = grupoComponente.get('usos') as FormArray;
                  const faseSeleccionada = this.fasesSelect.find(f => f.codigo == compoApo.faseCodigo);
                  const componenteSeleccionado = this.componentesSelect.find(c => c.codigo == compoApo.tipoComponenteCodigo);

                  if ( compoApo['registroCompleto'] !== undefined && compoApo['registroCompleto'] === true ) {
                    grupoAportante.get( 'estadoSemaforo' ).setValue( 'completo' )
                  };
                  if ( compoApo['registroCompleto'] !== undefined && compoApo['registroCompleto'] === false ) {
                    grupoAportante.get( 'estadoSemaforo' ).setValue( 'en-proceso' )
                  };

                  grupoComponente.get('componenteAportanteId').setValue(compoApo.componenteAportanteId);
                  grupoComponente.get('contratacionProyectoAportanteId').setValue(compoApo.contratacionProyectoAportanteId);
                  grupoComponente.get('fase').setValue(faseSeleccionada);
                  grupoComponente.get('componente').setValue(componenteSeleccionado);

                  compoApo.componenteUso.forEach(uso => {
                    const grupoUso = this.createUso();
                    const usoSeleccionado = this.usosSelect.find(u => u.codigo == uso.tipoUsoCodigo);

                    grupoUso.get('componenteUsoId').setValue(uso.componenteUsoId);
                    grupoUso.get('componenteAportanteId').setValue(uso.componenteAportanteId);
                    grupoUso.get('usoDescripcion').setValue(usoSeleccionado);
                    grupoUso.get('valorUso').setValue(uso.valorUso);

                    if ( grupoAportante.get('valorAportanteProyecto').value === 0 && grupoUso.get('valorUso').value === 0 ) {
                      grupoAportante.get( 'estadoSemaforo' ).setValue( 'sin-diligenciar' )
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

            this.idSolicitud = this.contratacionProyecto.contratacionId;

            console.log(this.contratacionProyecto);

          }, 1000);
        });


    });

  }

  getMunicipio() {
    if (this.router.getCurrentNavigation().extras.replaceUrl || this.router.getCurrentNavigation().extras.skipLocationChange === false) {
      this.router.navigate(['/solicitarContratacion']);
      return;
    }

    this.municipio = this.router.getCurrentNavigation().extras.state.municipio;

  }


  addUso(j: number, i: number) {
    const listaUsos = this.componentes(j).controls[i].get('usos') as FormArray;
    listaUsos.push(this.createUso());
  }

  createUso(): FormGroup {
    return this.fb.group({
      componenteUsoId: [],
      componenteAportanteId: [],
      usoDescripcion: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)])],
      valorUso: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)])
      ]
    });
  }

  addComponent(i: number) {
    const grupoComponente = this.createComponente();
    const listaUsos = grupoComponente.get('usos') as FormArray;
    listaUsos.push(this.createUso());
    this.componentes(i).push(grupoComponente);
  }

  createComponente(): FormGroup {
    return this.fb.group({
      componenteAportanteId: [],
      contratacionProyectoAportanteId: [],
      fase: [null, Validators.required],
      componente: [null, Validators.required],
      usos: this.fb.array([])
    });
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {

    let valoresCorrectos = true;

    this.contratacionProyecto.contratacionProyectoAportante = [];

    this.aportantes.controls.forEach(controlAportante => {
      const listaComponentes = controlAportante.get('componentes') as FormArray;


      const aportante: ContratacionProyectoAportante = {
        contratacionProyectoAportanteId: controlAportante.get('contratacionProyectoAportanteId').value,
        contratacionProyectoId: this.contratacionProyecto.contratacionProyectoId,
        proyectoAportanteId: controlAportante.get('proyectoAportanteId').value,
        valorAporte: controlAportante.get('valorAportanteProyecto').value,
        componenteAportante: [],

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
          componenteUso: [],

        };


        listaUsos.controls.forEach(controlUsos => {
          const componenteUso: ComponenteUso = {
            componenteUsoId: controlUsos.get('componenteUsoId').value,
            componenteAportanteId: componenteAportante.componenteAportanteId,
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
      }

      this.contratacionProyecto.contratacionProyectoAportante.push(aportante);
    });

    if (valoresCorrectos) {

      this.projectContractingService.createEditContratacionProyectoAportanteByContratacionproyecto(this.contratacionProyecto)
        .subscribe(
          respuesta => {
          this.openDialog('', respuesta.message);

          if (respuesta.code === '200') {
            this.router.navigate(['/solicitarContratacion/solicitud', this.contratacionProyecto.contratacionId]);
          }
          },
          err => this.openDialog('', err.message)
        );

    } else {
      this.openDialog('', 'El valor total es diferente a la suma del valor de los componentes');
    }

    console.log(this.contratacionProyecto);
  }
}
