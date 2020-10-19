import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { Dominio, CommonService, Respuesta } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ProcesoSeleccion, ProcesoSeleccionService, CronogramaSeguimiento } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { forkJoin, from } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { mergeMap, tap, toArray } from 'rxjs/operators';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-registrar-seguimiento-cronograma',
  templateUrl: './registrar-seguimiento-cronograma.component.html',
  styleUrls: ['./registrar-seguimiento-cronograma.component.scss']
})
export class RegistrarSeguimientoCronogramaComponent implements OnInit {

  listaTipoIntervencion: Dominio[] = [];
  listaEstadosSeguimiento: Dominio[] = [];

  addressForm = this.fb.group({
    tipoIntervencion: [],
    actividades: this.fb.array([
    // this.fb.group({
    //   fechaMonitoreo: [null, Validators.required],
    //   estadoActividad: [null, Validators.required],
    //   observación: [null, Validators.required],
    //   tipoIntervencion: [] 
    // })
  ])
});

  

  hasUnitNumber = false;

  
  editorStyle = {
    height: '100px',
    width: '300px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  maxDate: Date;
  idProcesoSeleccion: number = 0;
  listaCronograma: CronogramaSeguimiento[];

  constructor(
              private fb: FormBuilder,
              private commonService: CommonService,
              private activatedRoute: ActivatedRoute,
              private procesoSeleccionService: ProcesoSeleccionService,
              public dialog: MatDialog,
              private router: Router,

             ) 
  {
    this.maxDate = new Date();
  }

  createActividad(){
    return this.fb.group({
      procesoSeleccionCronogramaId: [],
      fechaMaxima: [null, Validators.required],
      fechaMonitoreo: [null, Validators.required],
      estadoActividad: [null, Validators.required],
      observacion: [null, Validators.required],
      descripcion: [null, Validators.required],
      estadoActividadInicialCodigo: []
    })
  }
  
  ngOnInit(): void {

    forkJoin([
      this.commonService.listaTipoIntervencion(),
      this.commonService.listaEstadoCronogramaSeguimiento()
    ]).subscribe( respuesta => {

      this.listaTipoIntervencion = respuesta[0];
      this.listaEstadosSeguimiento = respuesta[1]
    })

    this.activatedRoute.params.subscribe( parametro =>{
      this.idProcesoSeleccion = parametro['id'];

      this.procesoSeleccionService.listaActividadesByIdProcesoSeleccion( this.idProcesoSeleccion ).subscribe( lista => {

        let listaActividades = this.addressForm.get('actividades') as FormArray;

        console.log( lista );
        
        lista.forEach( cronograma => {
          let grupo = this.createActividad();
          
          grupo.get('procesoSeleccionCronogramaId').setValue( cronograma.procesoSeleccionCronogramaId );
          grupo.get('fechaMaxima').setValue( cronograma.fechaMaxima );
          grupo.get('descripcion').setValue( cronograma.descripcion );

          listaActividades.push( grupo );

        })

      })

    })
  }

  get actividades(){
    return this.addressForm.get('actividades') as FormArray;
  }


  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  }

  onSubmit() {

    let listaActividades = this.addressForm.get('actividades') as FormArray;
    this.listaCronograma = [];

    let i=0;
    listaActividades.controls.forEach( control => {

      let cronogramaSeguimiento: CronogramaSeguimiento = {
        
        observacion: control.get('observacion').value,
        estadoActividadInicialCodigo: control.get('estadoActividadInicialCodigo').value,
        estadoActividadFinalCodigo: control.get('estadoActividad').value.codigo,
        procesoSeleccionCronogramaId: control.get('procesoSeleccionCronogramaId').value,

      }
      this.listaCronograma.push( cronogramaSeguimiento );
      i++
    })

    from( this.listaCronograma )
      .pipe( mergeMap(cronograma => this.procesoSeleccionService.createEditarCronogramaSeguimiento( cronograma )
          .pipe(  
              tap()
          )
      ),
    toArray())
    .subscribe( respuesta => {
      let res = respuesta[0] as Respuesta
      this.openDialog("Cronograma", res.message); 
      if (res.code == "200"){
        console.log(respuesta);       
        this.router.navigate(["/seleccion"]);

      }
    }, error => {
      this.openDialog("Cronograma", error.message); 
    })

  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

}

