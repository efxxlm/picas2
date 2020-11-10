import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { ProcesoSeleccionService, ProcesoSeleccionCronograma } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { ActivatedRoute } from '@angular/router';
import { from } from 'rxjs';
import { mergeMap, tap, toArray } from 'rxjs/operators';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-tabla-cronograma',
  templateUrl: './tabla-cronograma.component.html',
  styleUrls: ['./tabla-cronograma.component.scss']
})
export class TablaCronogramaComponent implements OnInit {

  @Input() editMode: any = {};

  addressForm = this.fb.array([]);
  maxDate: Date;
  listaCronograma: ProcesoSeleccionCronograma[] = [];
  idProcesoSeleccion: number = 0;

  editorStyle = {
    height: '100px',
    width: '600px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  

  constructor(
              private fb: FormBuilder,
              private procesoSeleccionService: ProcesoSeleccionService,
              private activatedRoute: ActivatedRoute,
              public dialog: MatDialog,

             ) 
  {
    this.maxDate = new Date();
  }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe( parametro =>{
      this.idProcesoSeleccion = parametro['id'];

      this.procesoSeleccionService.listaActividadesByIdProcesoSeleccion( this.idProcesoSeleccion ).subscribe( lista => {

        let listaActividades = this.addressForm as FormArray;
        this.listaCronograma = lista;

        console.log( lista );
        
        lista.forEach( cronograma => {
          let grupo = this.crearActividad();
          
          grupo.get('procesoSeleccionCronogramaId').setValue( cronograma.procesoSeleccionCronogramaId );
          grupo.get('descripcion').setValue( cronograma.descripcion );
          grupo.get('fecha').setValue( cronograma.fechaMaxima );

          listaActividades.push( grupo );

        })

      })

    })
  }

  agregaFuente() {
    this.addressForm.push(this.crearActividad());
  }

  crearActividad(): FormGroup {
    return this.fb.group({
      procesoSeleccionCronogramaId: [],
      descripcion: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(500)
      ])],
      fecha: [null, Validators.required]
    });
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  onSubmit() {

    let listaActividades = this.addressForm as FormArray;
    this.listaCronograma = [];

    let i=0;
    listaActividades.controls.forEach( control => {
      let procesoSeleccionCronograma: ProcesoSeleccionCronograma = {
        procesoSeleccionCronogramaId: control.get('procesoSeleccionCronogramaId').value,
        descripcion: control.get('descripcion').value,
        fechaMaxima: control.get('fecha').value,
        procesoSeleccionId: this.idProcesoSeleccion,
        numeroActividad: i,

      }
      this.listaCronograma.push( procesoSeleccionCronograma );
      i++
    })

    from( this.listaCronograma )
      .pipe( mergeMap(cronograma => this.procesoSeleccionService.createEditarProcesoSeleccionCronograma( cronograma )
          .pipe(  
              tap()
          )
      ),
    toArray())
    .subscribe( respuesta => {
      let res = respuesta[0] as Respuesta
      if (res.code == "200")
        this.openDialog("", res.message);    
        console.log(respuesta);       
        
        //jflorez deshabilito el modo visualización
        //this.editMode.valor = false; 
    })

  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });  
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result)
      {
        location.reload();
      }
    }); 


  }

}
