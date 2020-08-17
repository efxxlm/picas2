import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormGroup, FormControl } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { ContratacionProyecto, ComponenteUso, ComponenteAportante, ContratacionProyectoAportante } from 'src/app/_interfaces/project-contracting';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-definir-fuentes-y-usos',
  templateUrl: './definir-fuentes-y-usos.component.html',
  styleUrls: ['./definir-fuentes-y-usos.component.scss']
})

export class DefinirFuentesYUsosComponent implements OnInit {

  idSolicitud: number;
  contratacionProyecto: ContratacionProyecto;

  addressForm = this.fb.group([]);

  fasesSelect: Dominio[] = [];

  componentesSelect: Dominio[] = [];

  usosSelect: Dominio[] = [];

  
  createFormulario(){
    return this.fb.group({
      aportantes: this.fb.array([])
    }); 
  }

  componentes( i: number ) {
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


             ) 
  { 

  }

  createAportante(){
    return this.fb.group({
      contratacionProyectoAportanteId: [],
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
        this.projectContractingService.getContratacionProyectoById( id )
      ])

      .subscribe( response => {

        this.fasesSelect = response[0];
        this.componentesSelect = response[1];
        this.usosSelect = response[2];
        this.contratacionProyecto = response[3];


        setTimeout(() => {

          this.contratacionProyecto.contratacionProyectoAportante.forEach( apo => {
            let grupoAportante = this.createAportante();
            let listaComponentes = grupoAportante.get('componentes') as FormArray;

            grupoAportante.get('valorAportanteProyecto').setValue( apo.valorAporte );

            if ( apo.componenteAportante.length > 0 ){
                apo.componenteAportante.forEach( compoApo => {
                    let grupoComponente = this.createComponente();
                    let listaUsos = grupoComponente.get('usos') as FormArray;

                    grupoComponente.get('componente').setValue( compoApo.tipoComponenteCodigo )

                    compoApo.componenteUso.forEach( uso => {
                        let grupoUso = this.createUso();

                        listaUsos.push( grupoUso );
                     })

                    listaComponentes.push( grupoComponente );
                  })
            }else{

              let grupoComponente = this.createComponente();
              let grupoUso = this.createUso();
              let listaUsos = grupoComponente.get('usos') as FormArray;

              listaUsos.push( grupoUso );

              listaComponentes.push( grupoComponente );
            }

            this.aportantes.push( grupoAportante );
          })

          this.idSolicitud = this.contratacionProyecto.contratacionId;

          console.log( this.contratacionProyecto );

        }, 1000);
      })

      
    });

  }


  addUso(i: number) {
    const control = this.addressForm.get('componentes') as FormArray;
    const listaUsos = control.controls[i].get('usos') as FormArray;

    listaUsos.push(this.createUso());
  }

  createUso(): FormGroup {
    return this.fb.group({
      componenteUsoId: [],
      usoDescripcion: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)])],
      valorUso: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)])
      ]
    });
  }

  addComponent( i: number ) {
    this.componentes( i ).push(this.createComponente());
  }

  createComponente(): FormGroup {
    return this.fb.group({
      componenteAportanteId:[],
      fase: [null, Validators.required],
      componente: [null, Validators.required],
      usos: this.fb.array([])
    });
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  onSubmit() {

    this.contratacionProyecto.contratacionProyectoAportante = [];

      this.aportantes.controls.forEach( controlAportante => {
        let listaComponentes = controlAportante.get('componentes') as FormArray;


        let aportante: ContratacionProyectoAportante = {
          contratacionProyectoAportanteId: controlAportante.get('contratacionProyectoAportanteId').value,
          valorAporte: controlAportante.get('valorAportanteProyecto').value,
          componenteAportante: [],

        }

        listaComponentes.controls.forEach( controlComponente => {
          let listaUsos = controlComponente.get('usos') as FormArray;

          let componenteAportante: ComponenteAportante = {
            componenteAportanteId: controlComponente.get('componenteAportanteId').value,
            contratacionProyectoAportanteId: aportante.contratacionProyectoAportanteId,
            tipoComponenteCodigo: controlComponente.get('componente').value ? controlComponente.get('componente').value.codigo : null ,
            componenteUso: [],

          }


          listaUsos.controls.forEach( controlUsos => {
            let componenteUso: ComponenteUso = {
              componenteUsoId: controlUsos.get('componenteUsoId').value,
              componenteAportanteId: componenteAportante.componenteAportanteId,
              tipoUsoCodigo: controlUsos.get('usoDescripcion').value ? controlUsos.get('usoDescripcion').value.codigo : null, 
              valorUso: controlUsos.get('valorUso').value,
            }

            componenteAportante.componenteUso.push( componenteUso );
          })

          aportante.componenteAportante.push( componenteAportante );

        })

        this.contratacionProyecto.contratacionProyectoAportante.push( aportante );
    })

    this.projectContractingService.createEditContratacionProyecto( this.contratacionProyecto )
      .subscribe( response => {
        console.log( response );
      })

    console.log(this.contratacionProyecto);
  }
}
