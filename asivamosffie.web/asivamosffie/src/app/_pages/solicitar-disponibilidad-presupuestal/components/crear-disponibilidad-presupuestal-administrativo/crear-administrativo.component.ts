import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
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
    objeto: [null, Validators.required],
    consecutivo: [null, Validators.required],

  })

  listaProyectos: ListConcecutivoProyectoAdministrativo[] = []
  listaAportantes: ListAdminProyect[] = []

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

  ) 
  {
  }

  ngOnInit(): void {

    this.budgetAvailabilityService.getListCocecutivoProyecto()
      .subscribe( lista => {
        this.listaProyectos = lista;
      })
  }

  changeProyecto(){

    let proyecto = this.formulario.get('consecutivo').value;
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

    let disponibilidad: DisponibilidadPresupuestal = {
      objeto: this.formulario.get('objeto').value,
      tipoSolicitudCodigo: '3',
     
      disponibilidadPresupuestalProyecto: []
      
    }

    let proyectoSeleccionado = this.formulario.get('consecutivo').value

    let proyecto: DisponibilidadPresupuestalProyecto = {
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
