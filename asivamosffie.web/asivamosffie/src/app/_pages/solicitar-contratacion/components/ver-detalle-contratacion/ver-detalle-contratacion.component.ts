import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Contratacion } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from '../../../../core/_services/projectContracting/project-contracting.service';

@Component({
  selector: 'app-ver-detalle-contratacion',
  templateUrl: './ver-detalle-contratacion.component.html',
  styleUrls: ['./ver-detalle-contratacion.component.scss']
})
export class VerDetalleContratacionComponent implements OnInit {

  contratacion: Contratacion;
  ELEMENT_DATA    : any[]    = [
    { titulo: 'Tipo de intervención', name: 'tipoIntervencionCodigo' },
    { titulo: 'Llave MEN', name: 'llaveMen' },
    { titulo: 'Región', name: 'usuarioModificacion' },
    { titulo: 'Departamento/ Municipio', name: 'localizacionIdMunicipio' },
    { titulo: 'Institución Educativa', name: 'institucionEducativa' },
    { titulo: 'Sede', name: 'sede' }
  ];
  displayedColumns: string[] = [     
    'tipoIntervencionCodigo',
    'llaveMen',
    'usuarioModificacion',
    'localizacionIdMunicipio',
    'institucionEducativa',
    'sede' 
  ];
  dataTable: any[] = [];

  constructor ( private activatedRoute: ActivatedRoute,
                private projectContractingSvc: ProjectContractingService ) {
    console.log( this.activatedRoute.snapshot.params.id );
    this.getProjectContracting( this.activatedRoute.snapshot.params.id );
  }

  ngOnInit(): void {
  }

  getProjectContracting ( id: any ) {
    this.projectContractingSvc.getContratacionByContratacionId( id )
      .subscribe( resp => {
        this.contratacion = resp;
        console.log( this.contratacion );
        for ( let contratacionProyecto of this.contratacion.contratacionProyecto ) {
          this.dataTable.push(
            {
              tipoIntervencionCodigo: contratacionProyecto.proyecto.tipoIntervencionCodigo,
              llaveMen:contratacionProyecto.proyecto.llaveMen,
              usuarioModificacion: contratacionProyecto.proyecto.usuarioModificacion,
              localizacionIdMunicipio: contratacionProyecto.proyecto.localizacionIdMunicipio,
              institucionEducativa: contratacionProyecto.proyecto.institucionEducativa.nombre,
              sede: contratacionProyecto.proyecto.sede.nombre
            }
          )
        }
      } );
  }

}
