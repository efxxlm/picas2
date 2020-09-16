import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { ProyectoAdministrativo, ProyectoAdministrativoAportante } from 'src/app/core/_services/project/project.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DisponibilidadPresupuestal, DisponibilidadPresupuestalProyecto, ListAdminProyect, ListConcecutivoProyectoAdministrativo } from 'src/app/_interfaces/budgetAvailability';

@Component({
  selector: 'app-crear-administrativo',
  templateUrl: './crear-administrativo.component.html',
  styleUrls: ['./crear-administrativo.component.scss']
})
export class CrearDisponibilidadPresupuestalAdministrativoComponent implements OnInit {

  formulario = this.fb.group({
    disponibilidadPresupuestalId: [],
    proyectoAdministrativoId: [],

    objeto: [null, Validators.required],
    consecutivo: [null, Validators.required],

  })

  listaProyectos: ListConcecutivoProyectoAdministrativo[] = []
  listaAportantes: ListAdminProyect[] = []
  objetoDispinibilidad: DisponibilidadPresupuestal = {}

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

  constructor(
    private fb: FormBuilder,
    private budgetAvailabilityService: BudgetAvailabilityService,
    public dialog: MatDialog,
    private router: Router,
    private activatedRoute: ActivatedRoute,

  ) 
  {
  }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe( parametros => {
      forkJoin([
        this.budgetAvailabilityService.getListCocecutivoProyecto(),
        this.budgetAvailabilityService.getDisponibilidadPresupuestalById( parametros.id )
      ]).subscribe( respuesta => {
          this.listaProyectos = respuesta[0];

          this.objetoDispinibilidad = respuesta[1];

          //console.log( this.objetoDispinibilidad );

          let proyecto = this.objetoDispinibilidad.disponibilidadPresupuestalProyecto[0];

          let proyectoSeleccionado = this.listaProyectos.find( p => p.proyectoId == proyecto.proyectoAdministrativoId );

          this.formulario.get('consecutivo').setValue( proyectoSeleccionado )
          this.formulario.get('objeto').setValue( this.objetoDispinibilidad.objeto )
          this.formulario.get('proyectoAdministrativoId').setValue( proyecto.proyectoAdministrativoId )
          this.formulario.get('disponibilidadPresupuestalId').setValue( this.objetoDispinibilidad.disponibilidadPresupuestalId )

          this.changeProyecto();

        })
    })

    
  }

  changeProyecto(){

    let proyecto = this.formulario.get('consecutivo').value;
    console.log( proyecto )
    this.budgetAvailabilityService.getAportantesByProyectoAdminId( proyecto.proyectoId )
      .subscribe( lista  => {
        this.listaAportantes = lista;
      })
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  enviarObjeto() {

    let aportante = this.listaAportantes[0];

    let disponibilidad: DisponibilidadPresupuestal = {
      disponibilidadPresupuestalId: this.formulario.get('disponibilidadPresupuestalId').value,
      objeto: this.formulario.get('objeto').value,
      tipoSolicitudCodigo: '3',
      valorSolicitud: aportante ? aportante.valorAporte : 0,
     
      disponibilidadPresupuestalProyecto: []
      
    }

    let proyectoSeleccionado = this.formulario.get('consecutivo').value

    let proyecto: DisponibilidadPresupuestalProyecto = {
       disponibilidadPresupuestalProyectoId: this.formulario.get('proyectoAdministrativoId').value,
      proyectoAdministrativoId: proyectoSeleccionado ? proyectoSeleccionado.proyectoId : null,
    }

    disponibilidad.disponibilidadPresupuestalProyecto.push( proyecto );

    this.budgetAvailabilityService.createOrEditProyectoAdministrtivo( disponibilidad )
      .subscribe( respuesta => {
        this.openDialog( '', respuesta.message )
        if ( respuesta.code == "200" )
          this.router.navigate(['/solicitarDisponibilidadPresupuestal'])
      })

     console.log( disponibilidad, this.formulario.get('consecutivo').value.proyectoId );
    
  }

}
