import { Component, OnInit } from '@angular/core';
import { Role } from 'src/app/models/auth/role';
import { UserRole } from 'src/app/models/auth/user-role';
import { AdminService } from 'src/app/services/admin.service';

declare const $: any;

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  users: UserRole[];
  loading = true;
  roles = Role;
  modalTitle: string;
  isEdit: boolean;
  selectedId: string;
  user: UserRole;

  constructor(private adminSvc: AdminService) {
  }

  ngOnInit(): void {
    this.adminSvc.getUsers().subscribe(users => {
      this.users = users;
      this.loading = false;
    });
  }

  onCreate(): void {
    this.modalTitle = 'Create New User';
    this.isEdit = false;
    this.user = new UserRole();
    $('#create-or-edit form').removeClass('was-validated').addClass('needs-validation');
    $('#create-or-edit').modal();
  }

  onEdit(id: string): void {
    this.modalTitle = 'Update User';
    this.isEdit = true;
    this.selectedId = id;
    this.user = this.users.find(x => x.id === id);
    $('#create-or-edit form').removeClass('was-validated').addClass('needs-validation');
    $('#create-or-edit').modal();
  }

  onCreateOrEdit() {
    if ($('#create-or-edit form')[0].checkValidity() === false) {
      $('#create-or-edit form').addClass('was-validated');
      return;
    }
    $('#create-or-edit').modal('toggle');
    this.user.role = parseInt(<any>this.user.role);

    if (this.isEdit) {
      this.adminSvc.updateUser(this.user).subscribe(user => {
      });
    } else {
      this.adminSvc.createUser(this.user).subscribe(user => {
        this.users.push(user);
      });
    }
  }

  onDelete(id: string): void {
    this.selectedId = id;
    $('#delete-confirmation').modal();
  }

  onDeleteConfirmation(): void {
    this.adminSvc.deleteUser(this.selectedId).subscribe(() => {
      this.users = this.users.filter(x => x.id !== this.selectedId);
    });
  }
}
